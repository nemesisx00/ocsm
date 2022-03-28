#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::*;
use crate::{
	cod::{
		advantages::BaseAdvantages,
		merits::Merit,
		tracks::Tracker,
		traits::{
			BaseAttributeType,
			BaseSkillType,
		},
		state::{
			CharacterAdvantages,
			CharacterAspirations,
			CharacterAttributes,
			CharacterBeats,
			CharacterExperience,
			CharacterMerits,
			CharacterSkills,
			CharacterSpecialties,
		},
		ctl2e::{
			details::DetailType as ctl2eDetailType,
			state::{
				ChangelingDetails,
			}
		},
		vtr2e::{
			advantages::TemplateAdvantages as vtr2eTemplateAdvantages,
			details::DetailType as vtr2eDetailType,
			disciplines::{
				Devotion,
				DisciplineType,
			},
			state::{
				KindredAdvantages,
				KindredDetails,
				KindredDevotions,
				KindredDisciplines,
				KindredTouchstones,
			}
		},
	}
};

// -----

/*
Memory capacity/usage isn't really an issue for this app. Even with all
the currently planned game systems implemented, I would guess the memory
footprint would still be trivial.

That said, there's no reason to waste memory with loose fragments of
game system data floating around that aren't being used in the currently
loaded game system.
*/

/// Reset every stateful value in the application, regardless of game system.
pub fn resetGlobalState<T>(cx: &Scope<T>)
{
	let characterAdvantages = use_atom_ref(cx, CharacterAdvantages);
	let characterAspirations = use_atom_ref(cx, CharacterAspirations);
	let characterAttributes = use_atom_ref(cx, CharacterAttributes);
	let characterBeats = use_atom_ref(cx, CharacterBeats);
	let characterExperience = use_set(cx, CharacterExperience);
	let characterMerits = use_atom_ref(cx, CharacterMerits);
	let characterSkills = use_atom_ref(cx, CharacterSkills);
	let characterSpecialties = use_atom_ref(cx, CharacterSpecialties);
	
	let changelingDetails = use_atom_ref(cx, ChangelingDetails);
	
	let kindredAdvantages = use_atom_ref(cx, KindredAdvantages);
	let kindredDetails = use_atom_ref(cx, KindredDetails);
	let kindredDevotions = use_atom_ref(cx, KindredDevotions);
	let kindredDisciplines = use_atom_ref(cx, KindredDisciplines);
	let kindredTouchstones = use_atom_ref(cx, KindredTouchstones);
	
	
	(*characterAdvantages.write()) = BaseAdvantages::default();
	(*characterAspirations.write()) = Vec::<String>::new();
	(*characterAttributes.write()) = BaseAttributeType::asMap();
	(*characterBeats.write()) = Tracker::new(5);
	characterExperience(0);
	(*characterMerits.write()) = Vec::<Merit>::new();
	(*characterSkills.write()) = BaseSkillType::asMap();
	(*characterSpecialties.write()) = Vec::<String>::new();
	
	(*changelingDetails.write()) = ctl2eDetailType::asMap();
	
	(*kindredAdvantages.write()) = vtr2eTemplateAdvantages::default();
	(*kindredDetails.write()) = vtr2eDetailType::asMap();
	(*kindredDevotions.write()) = Vec::<Devotion>::new();
	(*kindredDisciplines.write()) = BTreeMap::<DisciplineType, usize>::new();
	(*kindredTouchstones.write()) = Vec::<String>::new();
}
