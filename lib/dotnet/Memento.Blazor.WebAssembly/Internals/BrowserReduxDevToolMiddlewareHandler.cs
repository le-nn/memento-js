﻿using Memento.Blazor.Devtools;
using Memento.Core;
using Memento.Core.Executors;
using Memento.Core.Store;
using Microsoft.JSInterop;
using System.Collections.Immutable;
using System.Text.Json;

using static Memento.Core.Command;

namespace Memento.ReduxDevtool.Internals;

/// <remarks>
/// Reference to the redux devtool instrument.
/// https://github.com/zalmoxisus/redux-devtools-instrument/blob/master/src/instrument.js
/// </remarks>
class BrowserReduxDevToolMiddlewareHandler : MiddlewareHandler {
    IDisposable? _subscription1;
    IDisposable? _subscription2;

    readonly IDevtoolInteropHandler _interopHandler;
    readonly StoreProvider _storeProvider;
    readonly ConcatAsyncOperationExecutor _concatExecutor = new();
    readonly LiftedStore _liftedStore;
    readonly ThrottledExecutor<HistoryStateContextJson> _throttledExecutor = new();

    public BrowserReduxDevToolMiddlewareHandler(IServiceProvider provider, ReduxDevToolOption option) {
        _interopHandler = new DevToolJsInterop(
            (IJSRuntime)(provider.GetService(typeof(IJSRuntime))
                ?? throw new Exception("Prease register 'IJSRuntime' to ServiceProvider")
            ),
            HandleMessage
        );
        _storeProvider = (StoreProvider)(
            provider.GetService(typeof(StoreProvider))
                ?? throw new Exception("Prease register 'IJSRuntime' to ServiceProvider")
        );
        _liftedStore = new(_storeProvider, option) {
            SyncReqested = _throttledExecutor.Invoke
        };
        _subscription2 = _throttledExecutor.Subscribe(async sended => {
            await _interopHandler.SendAsync(null, sended);
        });
    }

    protected override async Task OnInitializedAsync() {
        await _liftedStore.ResetAsync();
        await _interopHandler.InitializeAsync(_storeProvider.CaptureRootState());

        _subscription1 = _storeProvider.Subscribe(e => {
            _ = _concatExecutor.ExecuteAsync(async () => {
                await SendAsync(e, e.RootState);
            });
        });
    }

    public async Task SendAsync(RootStateChangedEventArgs e, ImmutableDictionary<string, object> rootState) {
        if (e.StateChangedEvent.Command is ForceReplace) {
            return;
        }

        await (
            _liftedStore.PushAsync(e.StateChangedEvent, rootState)
                ?? Task.CompletedTask
        );
    }

    async void HandleMessage(string json) {
        try {
            var command = JsonSerializer.Deserialize<ActionItemFromDevtool>(
                json,
                new JsonSerializerOptions() {
                    PropertyNameCaseInsensitive = true,
                });

            if (command?.Payload?.TryGetValue("type", out var val) is true) {
                switch (val.ToString()) {
                    case "RESET":
                        await _liftedStore.ResetAsync();
                        break;
                    case "COMMIT":
                        await _liftedStore.CommitAsync();
                        break;
                    case "SWEEP":
                        await _liftedStore.SweepAsync();
                        break;
                    case "ROLLBACK":
                        await _liftedStore.RollbackAsync();
                        break;
                    case "PAUSE_RECORDING":
                        _liftedStore.IsPaused = command.Payload["status"].GetBoolean();
                        break;
                    case "REORDER_ACTION":
                        await _liftedStore.ReorderActionsAsync(
                            command.Payload["actionId"].GetInt32(),
                            command.Payload["beforeActionId"].GetInt32()
                        );
                        break;
                    case "JUMP_TO_STATE":
                    case "TOGGLE_ACTION":
                        _ = _liftedStore.SkipAsync(
                            command.Payload["id"].GetInt32()
                        );
                        break;
                    case "LOCK_CHANGES":
                        await _liftedStore.LockChangedAsync(command.Payload["status"].GetBoolean());
                        break;
                    case "JUMP_TO_ACTION":
                        _liftedStore.JumpTo(
                            command.Payload["actionId"].GetInt32()
                        );
                        break;
                    case "IMPORT_STATE":
                        await _liftedStore.ImportAsync(command.Payload["nextLiftedState"]);
                        break;
                }
            }
        }
        catch (Exception ex) {
            // TODO: Handle logger
            Console.WriteLine(ex.StackTrace);
        }
    }

    public override void Dispose() {
        _subscription1?.Dispose();
        _subscription2?.Dispose();
        _subscription1 = null;
        _subscription2 = null;
    }
}