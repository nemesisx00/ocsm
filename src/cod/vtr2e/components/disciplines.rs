#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	prelude::*,
	events::FormEvent
};
use crate::cod::{
	components::dots::{
		Dots,
		DotsProps,
	},
	vtr2e::{
		disciplines::{
			DisciplineType,
			Devotion,
			DevotionProperty,
		},
		state::{
			KindredDisciplines,
			KindredDevotions,
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

// -----

pub fn Devotions(scope: Scope) -> Element
{
	let devotionsRef = use_atom_ref(&scope, KindredDevotions);
	let devotions = devotionsRef.read();
	
	return scope.render(rsx!
	{
		div
		{
			class: "devotions entryListWrapper column",
			
			div { class: "entryListLabel", "Devotions" }
			
			div
			{
				class: "entryList column",
				
				(0..devotions.len()).map(|i|
				{
					let dev = &devotions[i];
					
					rsx!(scope, div
					{
						key: "{i}",
						class: "entry column",
						
						div
						{
							class: "row",
							
							div { class: "label first", "Name:" }
							input { r#type: "text", value: "{dev.name}", onchange: move |e| inputHandler(e, &scope, Some(i), DevotionProperty::Name) }
							div { class: "label second", "Cost:" }
							input { r#type: "text", value: "{dev.cost}", onchange: move |e| inputHandler(e, &scope, Some(i), DevotionProperty::Cost) }
						}
						
						div
						{
							class: "row",
							
							div { class: "label first", "Dice Pool:" }
							input { r#type: "text", value: "{dev.dicePool}", onchange: move |e| inputHandler(e, &scope, Some(i), DevotionProperty::DicePool) }
							div { class: "label second", "Reference:" }
							input { r#type: "text", value: "{dev.reference}", onchange: move |e| inputHandler(e, &scope, Some(i), DevotionProperty::Reference) }
						}
						
						div
						{
							class: "row",
							
							div { class: "label first", "Requirements:" }
							input { r#type: "text", value: "{dev.disciplines}", onchange: move |e| inputHandler(e, &scope, Some(i), DevotionProperty::Disciplines) }
						}
					})
				})
				
				div
				{
					class: "entry column",
					
					div
					{
						class: "row",
						
						div { class: "label first", "Name:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &scope, None, DevotionProperty::Name) }
						div { class: "label second", "Cost:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &scope, None, DevotionProperty::Cost) }
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Dice Pool:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &scope, None, DevotionProperty::DicePool) }
						div { class: "label second", "Reference:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &scope, None, DevotionProperty::Reference) }
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Requirements:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &scope, None, DevotionProperty::Disciplines) }
					}
				}
			}
		}
	});
}

fn inputHandler(e: FormEvent, scope: &Scope, index: Option<usize>, prop: DevotionProperty)
{
	let devotionsRef = use_atom_ref(&scope, KindredDevotions);
	let mut devotions = devotionsRef.write();
	
	match index
	{
		Some(i) =>
		{
			match prop
			{
				DevotionProperty::Cost => { devotions[i].cost = e.value.clone(); }
				DevotionProperty::DicePool => { devotions[i].dicePool = e.value.clone(); }
				DevotionProperty::Disciplines => { devotions[i].disciplines = e.value.clone(); }
				DevotionProperty::Name => { devotions[i].name = e.value.clone(); }
				DevotionProperty::Reference => { devotions[i].reference = e.value.clone(); }
			}
		}
		None =>
		{
			match prop
			{
				DevotionProperty::Cost => { devotions.push(Devotion { cost: e.value.clone(), ..Default::default() }); }
				DevotionProperty::DicePool => { devotions.push(Devotion { dicePool: e.value.clone(), ..Default::default() }); }
				DevotionProperty::Disciplines => { devotions.push(Devotion { disciplines: e.value.clone(), ..Default::default() }); }
				DevotionProperty::Name => { devotions.push(Devotion { name: e.value.clone(), ..Default::default() }); }
				DevotionProperty::Reference => { devotions.push(Devotion { reference: e.value.clone(), ..Default::default() }); }
			}
		}
	}
}
