# Open Character Sheet Manager ![GitHub](https://img.shields.io/github/license/nemesisx00/ocsm)

Open Character Sheet Manager (OCSM) is an open source cross-platform desktop application for conveniently managing TableTop RolePlaying Game (TTRPG) character sheets for a wide variety of game systems.

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

## What Is OCSM?

OCSM is an application intended to facilitate players' ability to enjoy playing their game of choice by taking (most) of the hassle out of managing their character information. Rather than dealing with bulky PDFs or difficult-to-organize plain text files, you can enter all of the most important information into meticulously organized and clearly labeled sections on a Sheet specifically designed for your favorite game system. OCSM will handle updating as many of the calculated traits as it can so you can focus on developing the character of your dreams.

It also can facilitate sharing characters, either between players or to keep your GM up to date. The character sheet files generated when you save your character are incredibly small, especially when compared to PDF alternatives. They are also essentially plain text documents so they are less likely to be blocked by spam filters or other security mechanisms.

## What OCSM Is Not

This application is not intended to be a replacement for official game system source material. It's a **Character Sheet** manager, not a game system manager. You will need to enter most, if not all, of the information you want in your sheet yourself. Thankfully, you can paste into most of the input fields. Also, with the addition of a Metadata layer, you only need to enter new information once at which point you will be able to reuse it across all your character sheets in that particular game system.

## Supported & Planned Game Systems

- Chronicles of Darkness
	- [x] Core Chronicles of Darkness
	- [x] Changeling: The Lost - Second Edition
	- [ ] Mage: The Awakening - Second Edition
		<!-- - Includes a Spellcasting Calculator to quickly determine your dice pool and paradox risk! -->
	- [ ] Vampire: The Requiem - Second Edition
- Dungeons & Dragons
	- [ ] 5th Edition
- Pathfinder
	- [ ] Second Edition
- World of Darkness
	- [ ] Vampire: The Masquerade V5

...and more to come in the future!

## Why a Character Sheet Manager?

In my experience, finding a high quality digital character sheet, regardless of format, for any game system is very difficult. Especially if you're looking for a free option. If you are lucky enough to find one for your chosen game system, most of the time they end up being PDFs which, while they can be of exceedingly high visual quality, suffer from the fact that interactive PDFs are clunky at the best of times. Not to mention that PDF is one of the least portable document formats in existence...

Since online communication tools like Discord, among many others, provide access to a much wider range of available games and players, I am very interested in digital resources for supporting this style of playing TTRPGs. I am also a huge proponent of free, open source software. So if I'm going to build something for myself, I also want to build it such that everyone else can use it too.

## Inside OCSM

OCSM is created using [Godot](https://godotengine.org) with scripts written in C#.

## Getting Started

OCSM isn't ready for release as an installable application just yet but you are more than welcome to try it out while I continue to work towards reaching version 1.0.0! Currently, the best way to do that is to compile it yourself. This process is relatively simple but it does require some tools to be installed beforehand.

#### Requirements

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Godot - .NET](https://godotengine.org)

#### Compiling, Running, and Testing

Now that you've got all the tools you'll need, the next step is to acquire the source code. The easiest way is to use git to clone this repository directly. If you're not sure how to do that, GitHub has provided very detailed instructions here: [Cloning a Repository](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository)

Now you're ready to get to it! All you need to do at this point is open up [Godot](https://godotengine.org) and import the project.

Once it's open in [Godot](https://godotengine.org), you can press F5 to run the application.

That's it! You may run into errors and you will definitely run into missing features. Unfortunately, that is inevitable at this stage of development.

## What Inspired Me to Create OCSM?

First, and foremost, this was a learning opportunity for me. Originally, I wanted a project that would push me to explore [Rust](https://www.rust-lang.org/) in greater detail. I am fascinated by the language: its focus on error prevention via strict adherence to deterministic syntax and type safety, built-in code tests, eschewing inheritance altogether, and, perhaps most importantly, its incredibly helpful compiler error messages!

Because I am particularly interested in TableTop Role Playing Games (TTRPGs), I thought an application to manage character sheets and data would be sufficiently complex to provide interesting development challenges and also produce a meaningful piece of software that people could use in the real world. Also, in my opinion, all the free options for character sheet management aren't great and all the great options aren't free. And I'm pretty sure none of them are available offline. So OCSM was created to fill that void as a free high quality offline desktop application.

In searching for [Rust](https://www.rust-lang.org/) GUI implementations, I happened upon [Tauri](https://tauri.studio) and my interest in writing a WebView2 desktop application was piqued. While that original version of the project, using [React](https://reactjs.org) to build the frontend, was absolutely functional, I was disappointed with the fact that more than 80% of the project was written in JavaScript rather than [Rust](https://www.rust-lang.org/), my primary focus.

More searching lead me to discover [Dioxus](https://dioxuslabs.com/) which provides most of the same functionality I was getting from [Tauri](https://tauri.studio/) along with the added benefit of writing the frontend code in [Rust](https://www.rust-lang.org/) as well. Also, because [Dioxus](https://dioxuslabs.com/)' design is intentionally modelled after [React](https://reactjs.org), it was relatively easy for me to get back up to speed.

After making considerable progress building the application with [Rust](https://www.rust-lang.org/) & [Dioxus](https://dioxuslabs.com/), I started realizing that, while it was a very educational experience, it was also just a lot of work to create & maintain new sheets. At the same time, I had started exploring game development using [Godot](https://godotengine.org) and realized [Godot](https://godotengine.org)'s UI scenes would be an acceptable replacement which would be less hassle in the long run.
