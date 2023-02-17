# Memento

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Flexible and easy unidirectional store library for state management with Dependency Injection for React or other frontend apps.

[DEMO](https://le-nn.github.io/memento-js/) with React in Typescript

This project is React binding of [Memento.NET](https://le-nn.github.io/memento-js/)

# Basic Concept

You can define stores inspired by MVU patterns such as Flux and Elm to observe state changes more detail.

Some are inspired by Elm and MVU.
And Redux and Flux pattern are same too, but memento is not Redux and Flux.

#### Features

* Less boilarplate and simple usage 
* Is not flux or redux
* Observe detailed status with command patterns and makes it easier to monitor what happened within the application 
* Immutable and Unidirectional data flow
* Multiple stores but manged by single provider, so can observe and manage as one state tree
* Less rules have been established
* You can choose between a simple store pattern or a dispatch reducer pattern like Flux or MVU

# Concepts and Data Flow

Note the concept is a bit different from Flux and Redux

<img width="800px" src="./Architecture.jpg"/>

## Rules

### Store pattern Rule

* mutate method mutates a store state

### Flux like pattern Rule

* To change state our app should Dispatch via Reducer in the action method
* Every Reducer that processes in the action will create new state to reflect the old state combined with the changes expected for the action.

### Rules that apply to both

* State should always be read-only.
* The UI then uses the new state to render its display.

## Overview

Examples

### The currently supported framework bindings are as follows

### Current packages and status

| Package Name    | Version | Lang       | Platform            | Package manager | Release Notes                      | Package provider                                       |
| --------------- | ------- | ---------- | ------------------- | --------------- | ---------------------------------- | ------------------------------------------------------ |
| memento.core    | 1.0.3   | TS/JS      | node.js 14 or later | npm or yarn     | [Notes](./release-notes.node.md)   | [npm](https://www.npmjs.com/package/memento.core)      |
| memento.react   | 1.0.4   | TS/JS      | node.js 14 or later | npm or yarn     | [Notes](./release-notes.node.md)   | [npm](https://www.npmjs.com/package/memento.react)     |

# Documentation

[Basic Concept with Typescript/Javascript](./docs/Tutorial.ts.md)

[React](./docs/React/GettingStandard.md)

# Demo

Here is a demo site built with React in Typescript.
[DEMO](https://le-nn.github.io/memento/)

# License
Designed with ♥ by le-nn. Licensed under the MIT License.
