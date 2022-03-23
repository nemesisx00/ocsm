#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use dioxus::events::{
	MouseEvent,
	FormEvent
};
use crate::cod::{
	lists::KeyedEntry,
	components::dots::{
		Dots,
		DotsProps,
	},
	vtr2e::{
		disciplines::{
			DisciplineType,
		},
		state::{
			KindredDisciplines,
			updateDiscipline,
		}
	}
};

pub fn Disciplines(scope: Scope) -> Element
{
	let disciplinesRef = use_atom_ref(&scope, KindredDisciplines);
	let disciplines = disciplinesRef.read();
	
	let dotsClass = "entry row";
	
	return scope.render(rsx!
	{
		div
		{
			class: "disciplines entryListWrapper column",
			
			div { class: "entryListLabel", "Disciplines" }
			
			div
			{
				class: "entryList column",
				
				Dots { class: dotsClass.to_string(), label: "Animalism".to_string(), max: 5, value: disciplines.animalism.value, handler: dotsHandler, handlerKey: DisciplineType::Animalism }
				Dots { class: dotsClass.to_string(), label: "Auspex".to_string(), max: 5, value: disciplines.auspex.value, handler: dotsHandler, handlerKey: DisciplineType::Auspex }
				Dots { class: dotsClass.to_string(), label: "Celerity".to_string(), max: 5, value: disciplines.celerity.value, handler: dotsHandler, handlerKey: DisciplineType::Celerity }
				Dots { class: dotsClass.to_string(), label: "Dominate".to_string(), max: 5, value: disciplines.dominate.value, handler: dotsHandler, handlerKey: DisciplineType::Dominate }
				Dots { class: dotsClass.to_string(), label: "Majesty".to_string(), max: 5, value: disciplines.majesty.value, handler: dotsHandler, handlerKey: DisciplineType::Majesty }
				Dots { class: dotsClass.to_string(), label: "Nightmare".to_string(), max: 5, value: disciplines.nightmare.value, handler: dotsHandler, handlerKey: DisciplineType::Nightmare }
				Dots { class: dotsClass.to_string(), label: "Obfuscate".to_string(), max: 5, value: disciplines.obfuscate.value, handler: dotsHandler, handlerKey: DisciplineType::Obfuscate }
				Dots { class: dotsClass.to_string(), label: "Protean".to_string(), max: 5, value: disciplines.protean.value, handler: dotsHandler, handlerKey: DisciplineType::Protean }
				Dots { class: dotsClass.to_string(), label: "Resilience".to_string(), max: 5, value: disciplines.resilience.value, handler: dotsHandler, handlerKey: DisciplineType::Resilience }
				Dots { class: dotsClass.to_string(), label: "Vigor".to_string(), max: 5, value: disciplines.vigor.value, handler: dotsHandler, handlerKey: DisciplineType::Vigor }
			}
		}
	});
}

fn dotsHandler(scope: &Scope<DotsProps<DisciplineType>>, clickedValue: usize)
{
	let finalValue = match clickedValue == scope.props.value
	{
		true => { clickedValue - 1 }
		false => { clickedValue }
	};
	
	match &scope.props.handlerKey
	{
		Some(dt) =>
		{
			updateDiscipline(scope, dt, finalValue);
		}
		None => {}
	}
}
