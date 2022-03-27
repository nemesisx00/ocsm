# Open Character Sheet Manager

Open Character Sheet Manager (OCSM) is an open source cross-platform desktop application for conveniently managing TableTop RolePlaying Game (TTRPG) character sheets for a wide variety of game systems.

## Table of Contents

- [Supported & Planned Game Systems](https://github.com/nemesisx00/ocsm#supported--planned-game-systems)
- [Inside OCSM](https://github.com/nemesisx00/ocsm#inside-ocsm)
- [Getting Started](https://github.com/nemesisx00/ocsm#getting-started)
	- [Requirements](https://github.com/nemesisx00/ocsm#requirements)
	- [Helpful Utilities](https://github.com/nemesisx00/ocsm#helpful-utilities)
	- [Compiling, Running, and Testing](https://github.com/nemesisx00/ocsm#compiling-running-and-testing)
- [What is OCSM?](https://github.com/nemesisx00/ocsm#what-is-ocsm)
- [What OCSM is Not](https://github.com/nemesisx00/ocsm#what-ocsm-is-not)
- [Why a character sheet manager?](https://github.com/nemesisx00/ocsm#why-a-character-sheet-manager)
- [Okay, but... ...why?](https://github.com/nemesisx00/ocsm#okay-but-why)
- [Coding Style](https://github.com/nemesisx00/ocsm#coding-style)

## Supported & Planned Game Systems

- Chronicles of Darkness
	- [x] Changeling: The Lost - Second Edition
	- [ ] Mage: The Awakening - Second Edition
	- [x] Vampire: The Requiem - Second Edition
- Dungeons & Dragons
	- [ ] 5th Edition
- World of Darkness
	- [ ] Vampire: The Masquerade V5

## Inside OCSM

OCSM is written almost entirely in [Rust](https://www.rust-lang.org/). The [Dioxus](https://dioxuslabs.com/) library is utilized to create a desktop WebView2 GUI driven by WASM, leveraging [Rust](https://www.rust-lang.org/)'s type safety on the frontend. Rounding things out, the CSS is generated with the [Stylus](https://stylus-lang.com/) CSS preprocessor.

## Getting Started

OCSM isn't ready for release as an installable application just yet but you are more than welcome to try it out while I continue to work towards reaching version 1.0.0! Currently, the best way to do that is to compile it yourself. This process is relatively simple but it does require some tools to be installed beforehand.

#### Requirements

- [Rust](https://www.rust-lang.org/)
- [Node.js](https://nodejs.org)

You may be surprised to see [Node.js](https://nodejs.org) in the list, considering this is primarily a [Rust](https://www.rust-lang.org/) application. But managing CSS files can be a very verbose ordeal and a CSS Preprocessor like [Stylus](https://stylus-lang.com/) makes it so much easier. NPM is used to manage the installation of [Stylus](https://stylus-lang.com/).

#### Helpful Utilities

There are a few utilities that are not strictly required but can make your life a lot easier and I recommend checking them out:

- [cargo-edit](https://github.com/killercup/cargo-edit)
- [cargo-script](https://github.com/DanielKeep/cargo-script)

#### Compiling, Running, and Testing

Now that you've got all the tools you'll need, the next step is to acquire the source code. The easiest way is to use git to clone this repository directly. If you're not sure how to do that, GitHub has provided very detailed instructions here: [Cloning a Repository](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository)

Now you're ready to compile! Well, almost. Start by opening your preferred command line interface and navigate to the project directory. Download and install [Stylus](https://stylus-lang.com/) by running:
```
npm install
```

Ok, now you're *really* ready to compile! The first step is running [Stylus](https://stylus-lang.com/) to generate the CSS:
```
npm run-script stylus
```

Now all that's left is to compile the [Rust](https://www.rust-lang.org/) code:
```
//If you installed Cargo Script; This runs Stylus first before compiling
cargo script compile

//Otherise
cargo build
```

You can also build and automatically run the application in one step:
```
//If you installed Cargo Script; This runs Stylus first before compiling
cargo script run

//Otherwise
cargo run
```

Finally, you can run all the tests in the project:
```
cargo test
```

I will note that, while the compile and run scripts are convenient, they lose the very useful coloration that [Rust](https://www.rust-lang.org/)'s compiler utilizes in its output. So, if you're trying to work through compiler errors, I recommend chaining the stylus script with `cargo run` instead, like this:
```
npm run-script stylus && cargo run
```

That's it!

## What is OCSM?

OCSM is an application intended to facilitate players' ability to enjoy playing their game of choice by taking (most) of the hassle out of managing their character information. Rather than dealing with bulky PDFs or difficult-to-organize plain text files, you can enter all of the most important information into meticulously organized and clearly labeled sections on a Sheet specifically designed for your favorite game system. OCSM will handle updating as many of the calculated traits as it can so you can focus on developing the character of your dreams.

It also can facilitate sharing characters, either between players or to keep your GM up to date. The character sheet files generated when you save your character are incredibly small, especially when compared to PDF alternatives. They are also essentially plain text documents so they are less likely to be blocked by spam filters or other security mechanisms.

## What OCSM is Not

This application is not intended to be a replacement for game system source books. It's a **Character Sheet** manager, not a game system manager. You will need to enter all the information you want in your sheet yourself. Thankfully, you can paste into most of the input fields.

## Why a Character Sheet Manager?

In my experience, finding a high quality digital character sheet, regardless of format, for any game system is very difficult. Especially if you're looking for a free option. If you are lucky enough to find one for your chosen game system, most of the time they end up being PDFs which, while they can be of exceedingly high visual quality, suffer from the fact that interactive PDFs are clunky at the best of times. Not to mention that PDF is one of the least portable document formats in existence...

Since online communication tools like Discord, among many others, provide access to a much wider range of available games and players, I am very interested in digital resources for supporting this style of playing TTRPGs. I am also a huge proponent of free, open source software. So I thought, why not bring those two things together to make something that people can actually *use*?

## Okay, But... ...Why?

First, and foremost, this was a learning opportunity for me. I wanted a project that would push me to explore [Rust](https://www.rust-lang.org/) in greater detail. I am fascinated by the language: its focus on error prevention via strict adherence to deterministic syntax and type safety, built-in code tests, eschewing inheritance altogether, and, perhaps most importantly, its incredibly helpful compiler error messages!

In searching for GUI libraries, I happened upon [Tauri](https://tauri.studio) and my interest in writing a WebView2 desktop application was piqued. While that original version of the project, using [React](https://reactjs.org) to build the frontend, was absolutely functional, I was disappointed with the fact that more than 80% of the project was written in JavaScript rather than [Rust](https://www.rust-lang.org/), my primary focus.

Another search lead me to discovering [Dioxus](https://dioxuslabs.com/) which provided most of the same functionality I was already getting from [Tauri](https://tauri.studio/) along with the added benefit of writing the frontend code in [Rust](https://www.rust-lang.org/) as well. Also, because [Dioxus](https://dioxuslabs.com/)' design is intentionally modelled after [React](https://reactjs.org), it was relatively easy for me to get back up to speed.

## Coding Style

For people familiar with [Rust](https://www.rust-lang.org/), a brief glance at the source code will reveal that I'm not sticking very close to the idiomatic [Rust](https://www.rust-lang.org/) coding style. While it is primarily about maintaining readability, coding style is also one of the few ways in which software developers can express themselves directly in their work. So I take issue with languages that try to prescribe any specific coding style, such as adding default compiler warnings when you don't use snake_case...

As for me, I hate snake_case, love hard tabs, and like my braces to line up vertically. Sorry, not sorry. With more than a decade of professional software development under my belt, I've put a lot of thought into my preferences for coding style and they are unlikely to change any time soon. I tend to use PascalCase for static variables, constants, structs/classes/objects, and traits/interfaces. I use camelCase for pretty much everything else. Even though UI components in [Dioxus](https://dioxuslabs.com/) are written as functions, I still think of them like a struct/class/object so I use PascalCase when naming them.
