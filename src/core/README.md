# memento.core

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Flexible and easy unidirectional store library for state management with Dependency Injection for React or other frontend apps.

You can define stores inspired by MVU patterns such as Flux and Elm to observe state changes more detail.

Some are inspired by Elm and MVU.
And Redux and Flux pattern are same too, but memento is not Redux and Flux.

Details and Docs on [Github](https://github.com/le-nn/memento-js)

#### Features

* Less boilarplate and simple usage 
* Is not flux or redux
* Observe detailed status with command patterns and makes it easier to monitor what happened within the application 
* Immutable and Unidirectional data flow
* Multiple stores but manged by single provider, so can observe and manage as one state tree
* Less rules have been established
* You can choose between a simple store pattern or a dispatch reducer pattern like Flux or MVU

# Code overview

Here is a sample of Flux like store pattern

```ts
import { meta, FluxStore, Message } from "memento.react";

const simpleState = {
    count: 0,
};

type SimpleCounterState = typeof simpleState

class Increment extends Message { }
class Decrement extends Message { }

@meta({ name: "FluxCounterStore" })
export class FluxCounterStore extends FluxStore<SimpleCounterState> {
    constructor() {
        super(simpleState, FluxCounterStore.mutation);
    }

    static mutation(state: SimpleCounterState, message: Message): SimpleCounterState {
        switch (message.comparer) {
            case Increment:
                return {
                    count: state.count + 1
                }
            case Decrement:
                return {
                    count: state.count - 1
                }
            default: throw new Error()
        }
    }

    async increment() {
        this.mutate(new Increment())
    }

    async decrement() {
        this.mutate(new Decrement())
    }
}

```

### Usage

```ts
export const provider = createProvider({
    stores: [
        FluxAsyncCounterStore,
    ]
})

const store = provider.resolve(FluxAsyncCounterStore)

store.subscribe(e => {
    console.log(e.present)
})

console.log(store.state)

store.increment()
store.increment()
store.increment()
store.decrement()
store.increment()
store.increment()
store.decrement()
store.decrement()
store.increment()
store.increment()
```

store.subscribe can be exptected output following

```json
{ "count": 0 }
{ "count": 1 }
{ "count": 2 }
{ "count": 3 }
{ "count": 2 }
{ "count": 3 }
{ "count": 4 }
{ "count": 3 }
{ "count": 2 }
{ "count": 3 }
{ "count": 4 }
```

# License
Designed with ♥ le-nn. Licensed under the MIT License.
