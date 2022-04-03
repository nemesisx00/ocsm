#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	Scope,
	use_atom_ref,
};
use serde::{
	Deserialize,
	Serialize,
};
use crate::{
	cod::{
		enums::{
			CoreAttribute,
			CoreSkill,
		},
		mta2e::{
			enums::{
				Arcana,
				SpellFactor,
				SpellPractice,
			},
			state::{
				MageActiveSpells,
				MageArcana,
				MageDedicatedTools,
				MageObsessions,
				MagePraxes,
				MageRotes,
				MageSpells,
			},
		},
		structs::CoreCharacter,
	},
	core::state::StatefulTemplate,
};

/// Data structure defining a Mage: The Awakening 2e Mage.
#[derive(Clone, Debug, Default, Deserialize, Serialize)]
pub struct Mage
{
	#[serde(default)]
	pub coreCharacter: CoreCharacter,
	
	#[serde(default)]
	pub activeSpells: Vec<String>,
	
	#[serde(default)]
	pub arcana: Vec<(Arcana, usize)>,
	
	#[serde(default)]
	pub dedicatedTools: Vec<String>,
	
	#[serde(default)]
	pub obsessions: Vec<String>,
	
	#[serde(default)]
	pub praxes: Vec<Praxis>,
	
	#[serde(default)]
	pub rotes: Vec<Rote>,
	
	#[serde(default)]
	pub spells: Vec<Spell>,
}

impl StatefulTemplate for Mage
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.pull(cx);
		
		let activeSpells = use_atom_ref(cx, MageActiveSpells);
		let arcana = use_atom_ref(cx, MageArcana);
		let dedicatedTools = use_atom_ref(cx, MageDedicatedTools);
		let obsessions = use_atom_ref(cx, MageObsessions);
		let praxes = use_atom_ref(cx, MagePraxes);
		let rotes = use_atom_ref(cx, MageRotes);
		let spells = use_atom_ref(cx, MageSpells);
		
		self.activeSpells = activeSpells.read().clone();
		self.arcana = arcana.read().clone();
		self.dedicatedTools = dedicatedTools.read().clone();
		self.obsessions = obsessions.read().clone();
		self.praxes = praxes.read().clone();
		self.rotes = rotes.read().clone();
		self.spells = spells.read().clone();
		
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.push(cx);
		
		self.validate();
		
		let activeSpells = use_atom_ref(cx, MageActiveSpells);
		let arcana = use_atom_ref(cx, MageArcana);
		let dedicatedTools = use_atom_ref(cx, MageDedicatedTools);
		let obsessions = use_atom_ref(cx, MageObsessions);
		let praxes = use_atom_ref(cx, MagePraxes);
		let rotes = use_atom_ref(cx, MageRotes);
		let spells = use_atom_ref(cx, MageSpells);
		
		(*activeSpells.write()) = self.activeSpells.clone();
		(*arcana.write()) = self.arcana.clone();
		(*dedicatedTools.write()) = self.dedicatedTools.clone();
		(*obsessions.write()) = self.obsessions.clone();
		(*praxes.write()) = self.praxes.clone();
		(*rotes.write()) = self.rotes.clone();
		(*spells.write()) = self.spells.clone();
	}
	
	fn validate(&mut self)
	{
		
	}
}

// --------------------------------------------------

/// Data structure defining a single Praxis spell.
#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub struct Praxis
{
	#[serde(default)]
	pub arcanum: Option<Arcana>,
	
	#[serde(default)]
	pub level: usize,
	
	#[serde(default)]
	pub name: String,
}

// --------------------------------------------------

/// Data structure defining a single Rote spell.
#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub struct Rote
{
	#[serde(default)]
	pub arcanum: Option<Arcana>,
	
	#[serde(default)]
	pub creator: String,
	
	#[serde(default)]
	pub level: usize,
	
	#[serde(default)]
	pub name: String,
	
	#[serde(default)]
	pub skill: Option<CoreSkill>,
}

// --------------------------------------------------

/// Data structure defining a single Spell, whether improvized, Praxis, Rote, or otherwise.
#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub struct Spell
{
	#[serde(default)]
	pub arcanum: Option<Arcana>,
	
	#[serde(default)]
	pub intent: String,
	
	#[serde(default)]
	pub effects: String,
	
	#[serde(default)]
	pub name: String,
	
	#[serde(default)]
	pub practice: Option<SpellPractice>,
	
	#[serde(default)]
	pub primaryFactor: Option<SpellFactor>,
	
	#[serde(default)]
	pub withstand: Option<CoreAttribute>,
}
