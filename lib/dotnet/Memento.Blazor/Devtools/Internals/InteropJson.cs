﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Memento.Blazor.Devtools;

public record StoreAction {
    [JsonPropertyName("action")]
    public required ActionItem Action { get; init; }

    [JsonPropertyName("timestamp")]
    public required long Timestamp { get; init; }

    [JsonPropertyName("stack")]
    public required object Stack { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }
}

public record ActionItem(
    [property:JsonPropertyName("type")]
    string? Type,
    [property:JsonPropertyName("payload")]
    object? Payload
);

public record HistoryStateContextJson {
    [JsonPropertyName("actionsById")]
    public required Dictionary<int, StoreAction> ActionsById { get; init; }

    [JsonPropertyName("computedStates")]
    public required ImmutableArray<ComputedState> ComputedStates { get; init; }

    [JsonPropertyName("currentStateIndex")]
    public required int CurrentStateIndex { get; init; }

    [JsonPropertyName("nextActionId")]
    public required int NextActionId { get; init; }

    [JsonPropertyName("skippedActionIds")]
    public required ImmutableArray<int> SkippedActionIds { get; init; }

    [JsonPropertyName("stagedActionIds")]
    public required ImmutableArray<int> StagedActionIds { get; init; }
}

public record ComputedState(
    [property:JsonPropertyName("state")]
    object State
);