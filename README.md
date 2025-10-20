<h1 align="center">Open Character Sheet Manager</h1>

<div align="center" width="100%">
	<img alt="GitHub" src="https://img.shields.io/github/license/nemesisx00/ocsm" />
	<img alt="GitHub Workflow Status" src="https://img.shields.io/github/actions/workflow/status/nemesisx00/ocsm/rust.yml" />
	<img alt="GitHub contributors" src="https://img.shields.io/github/contributors/nemesisx00/ocsm" />
	<img alt="GitHub last commit" src="https://img.shields.io/github/last-commit/nemesisx00/ocsm" />
	<!-- <img alt="GitHub commit activity" src="https://img.shields.io/github/commit-activity/m/nemesisx00/ocsm" /> -->
	<img alt="Discord" src="https://img.shields.io/discord/1084182068965158912">
</div>

<p align="center">
Open Character Sheet Manager (OCSM) is an open source cross-platform desktop application for conveniently managing TableTop RolePlaying Game (TTRPG) character sheets for a wide variety of game systems.
</p>

&nbsp;

## Table of Contents

- [What is OCSM?](https://github.com/nemesisx00/ocsm#what-is-ocsm)
- [What OCSM is Not](https://github.com/nemesisx00/ocsm#what-ocsm-is-not)
- [Supported & Planned Game Systems](https://github.com/nemesisx00/ocsm#supported--planned-game-systems)
- [Why a Character Sheet Manager?](https://github.com/nemesisx00/ocsm#why-a-character-sheet-manager)
- [Inside OCSM](https://github.com/nemesisx00/ocsm#inside-ocsm)
- [Getting Started](https://github.com/nemesisx00/ocsm#getting-started)
	- [Requirements](https://github.com/nemesisx00/ocsm#requirements)
	- [Compiling, Running, and Testing](https://github.com/nemesisx00/ocsm#compiling-running-and-testing)
- [What Inspired Me to Create OCSM?](https://github.com/nemesisx00/ocsm#what-inspired-me-to-create-ocsm)

&nbsp;

----

&nbsp;

## What Is OCSM?

OCSM is an application intended to facilitate players' ability to enjoy playing their game of choice by taking (most) of the hassle out of managing their character information. Rather than dealing with bulky PDFs or difficult-to-organize plain text files, you can enter all of the most important information into meticulously crafted and clearly labeled sections on a Sheet specifically designed for your favorite game system. OCSM will handle updating as many of the calculated traits as it can so you can focus on developing the character of your dreams.

It also can facilitate sharing characters, either between players or to keep your GM up to date. The character sheet files generated when you save your character are incredibly small, especially when compared to PDF alternatives. They are also essentially plain text documents so they are less likely to be blocked by spam filters or other security mechanisms.

&nbsp;

## What OCSM Is Not

This application is not intended to be a replacement for official game system source material. It's a **Character Sheet** manager, not a game system manager. You will need to enter most, if not all, of the information you want in your sheet yourself. <!-- Though, with the addition of a Metadata layer, you will only need to enter new information once, at which point you will be able to reuse it across all your character sheets in that particular game system. -->

&nbsp;

----

&nbsp;

## Supported & Planned Game Systems

- Chronicles of Darkness
	- [x] Core Chronicles of Darkness
	- [ ] Changeling: The Lost - Second Edition
	- [ ] Mage: The Awakening - Second Edition
		<!-- - Includes a Spellcasting Calculator to quickly determine your dice pool and paradox risk! -->
	- [x] Vampire: The Requiem - Second Edition
- Dungeons & Dragons
	- [ ] 5th Edition
- Lancer
	- [ ] First Edition
- Pathfinder
	- [ ] Second Edition
- World of Darkness
	- [ ] Vampire: The Masquerade V5

...and more to come in the future!

&nbsp;

## Why a Character Sheet Manager?

In my experience, finding a high quality digital character sheet, regardless of format, for any game system is very difficult. Especially if you're looking for a free option. If you are lucky enough to find one for your chosen game system, most of the time they end up being PDFs which, while they can be of exceedingly high visual quality, suffer from the fact that interactive PDFs are clunky at the best of times. Not to mention that PDF is one of the least portable document formats in existence...

Since online communication tools like Discord, among many others, provide access to a much wider range of available games and players, I am very interested in digital resources for supporting this style of playing TTRPGs. I am also a huge proponent of free, open source software. So if I'm going to build something for myself, I also want to build it such that everyone else can use it too.

&nbsp;

----

&nbsp;

## Inside OCSM

OCSM is built in [Rust](https://rust-lang.org), relying on [Freya](https://freyaui.dev) to draw the GUI. Linux is the target platform but it should be compatible with any platform that [Freya](https://freyaui.dev) also supports.

&nbsp;

## Getting Started

OCSM isn't ready for release just yet but you are more than welcome to try it out while I continue to work towards reaching version 1.0.0! Currently, the best way to do that is to compile it yourself. This process is relatively simple but it does require some tools to be installed beforehand.

#### Requirements

- [Rust](https://rust-lang.org/learn/get-started)

Because [Freya](https://freyaui.dev) relies on [Skia](https://skia.org) to actually draw the UI, you may need to install some extra development libraries depending on your environment. The fastest way to find out what it is you are missing is to attempt to compile the project. Either it will compile or it will inform you of what is missing.

#### Compiling, Running, and Testing

Now that you've got all the tools you'll need, the next step is to acquire the source code. The easiest way is to use git to clone this repository directly. If you're not sure how to do that, GitHub has provided very detailed instructions here: [Cloning a Repository](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository)

Now you're ready to get to it! Verify that everything is installed correctly by navigating to the folder in a console and entering the following command to build and run the release build of OCSM:

```
cargo run --release
```

That's it!

Note: At the moment, there is an issue with Freya where running the debug build will result in excessive lag in the UI. It still functions but, for the time being, it is recommended to only run the release build of OCSM.

If you are so inclined, you can also contribute and submit a Pull Request. If you're new to [Rust](https://rust-lang.org), you should take some time to become more familiar with [Cargo](https://doc.rust-lang.org/cargo) as well.

&nbsp;

----

&nbsp;

## What Inspired Me to Create OCSM?

First, and foremost, this was a learning opportunity for me. Originally, I wanted a project that would push me to explore [Rust](https://rust-lang.org) in greater detail. I am fascinated by the language: its focus on error prevention via strict adherence to deterministic syntax and type safety, built-in code tests, eschewing inheritance altogether, and, perhaps most importantly, its incredibly helpful compiler error messages!

Because I am particularly interested in TableTop Role Playing Games (TTRPGs), I thought an application to manage character sheets and data would be sufficiently complex to provide interesting development challenges and also produce a meaningful piece of software that people could use in the real world. Also, in my opinion, all the free options for character sheet management aren't great and all the great options aren't free. And I'm pretty sure none of them are available offline. So OCSM was created to fill that void as a free high quality offline desktop application.

The journey to today has brought the project into contact with numerous GUI solutions:

- [Tauri](https://tauri.studio)
- [Dioxus](https://dioxuslabs.com)
- [Godot](https://godotengine.org)
- [gtk-rs](https://gtk-rs.org)

All of these have their strengths and their weaknesses and using each of them has been a beneficial learning experience for me. As for OCSM, I have settled upon using [Freya](https://freyaui.dev) for the foreseeable future, both because it allows the project to be written purely in [Rust](https://rust-lang.org) and because it leverages the best parts of [Dioxus](https://dioxuslabs.com) without the complications which result from using a web frontend for an offline desktop application.

Anyway, thank you for making it all the way through my rambling!
