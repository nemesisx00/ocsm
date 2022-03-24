#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	prelude::*,
	events::FormEvent
};
use strum::IntoEnumIterator;
use crate::cod::{
	components::dots::{
		Dots,
		DotsProps,
	},
	vtr2e::{
		disciplines::{
			Discipline,
			DisciplineType,
			Devotion,
			DevotionField,
		},
		state::{
			KindredDisciplines,
			KindredDevotions,
		}
	}
};

pub fn Disciplines(cx: Scope) -> Element
{
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let disciplines = disciplinesRef.read();
	
	let dotsClass = "entry row";
	
	let mut optionNames = Vec::<String>::new();
	for dt in DisciplineType::iter()
	{
		optionNames.push(dt.as_ref().to_string());
	}
	
	return cx.render(rsx!
	{
		div
		{
			class: "disciplines entryListWrapper column",
			
			div { class: "entryListLabel", "Disciplines" }
			
			div
			{
				class: "entryList column",
				
				disciplines.iter().enumerate().map(|(i, d)| rsx!(cx, Dots { key: "{i}", class: dotsClass.to_string(), label: d.name.clone(), max: 5, value: d.value, handler: dotsHandler, handlerKey: i }))
				
				div
				{
					class: "entry row",
					
					select
					{
						onchange: move |e| selectHandler(e, &cx),
						
						option { value: "", selected: "true", "Choose a Discipline" }
						
						optionNames.iter().enumerate().map(|(i, name)| rsx!(cx, option
						{
							option { key: "{i}", value: "{name}", "{name}" }
						}))
					}
				}
			}
		}
	});
}

fn dotsHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let mut disciplines = disciplinesRef.write();
	
	match &cx.props.handlerKey
	{
		Some(index) => { disciplines[*index].value = clickedValue; }
		None => {}
	}
}

fn selectHandler(e: FormEvent, cx: &Scope)
{
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let mut disciplines = disciplinesRef.write();
	
	disciplines.push(Discipline
	{
		name: e.value.clone(),
		..Default::default()
	});
}

// -----

pub fn Devotions(cx: Scope) -> Element
{
	let devotionsRef = use_atom_ref(&cx, KindredDevotions);
	let devotions = devotionsRef.read();
	
	return cx.render(rsx!
	{
		div
		{
			class: "devotions entryListWrapper column",
			
			div { class: "entryListLabel", "Devotions" }
			
			div
			{
				class: "entryList column",
				
				devotions.iter().enumerate().map(|(i, dev)| rsx!(cx, div
				{
					class: "entry column",
					
					div
					{
						class: "row",
						
						div { class: "label first", "Name:" }
						input { r#type: "text", value: "{dev.name}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Name) }
						div { class: "label second", "Cost:" }
						input { r#type: "text", value: "{dev.cost}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Cost) }
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Dice Pool:" }
						input { r#type: "text", value: "{dev.dicePool}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::DicePool) }
						div { class: "label second", "Action:" }
						input { r#type: "text", value: "{dev.action}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Action) }
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Requirements:" }
						input { r#type: "text", value: "{dev.disciplines}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Disciplines) }
						div { class: "label second", "Reference:" }
						input { r#type: "text", value: "{dev.reference}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Reference) }
					}
				}))
				
				div
				{
					class: "entry column",
					
					div
					{
						class: "row",
						
						div { class: "label first", "Name:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &cx, None, DevotionField::Name) }
						div { class: "label second", "Cost:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &cx, None, DevotionField::Cost) }
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Dice Pool:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &cx, None, DevotionField::DicePool) }
						div { class: "label second", "Action:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &cx, None, DevotionField::Action) }
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Requirements:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &cx, None, DevotionField::Disciplines) }
						div { class: "label second", "Reference:" }
						input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &cx, None, DevotionField::Reference) }
					}
				}
			}
		}
	});
}

fn inputHandler(e: FormEvent, cx: &Scope, index: Option<usize>, prop: DevotionField)
{
	let devotionsRef = use_atom_ref(&cx, KindredDevotions);
	let mut devotions = devotionsRef.write();
	
	match index
	{
		Some(i) =>
		{
			match prop
			{
				DevotionField::Action => { devotions[i].action = e.value.clone(); }
				DevotionField::Cost => { devotions[i].cost = e.value.clone(); }
				DevotionField::DicePool => { devotions[i].dicePool = e.value.clone(); }
				DevotionField::Disciplines => { devotions[i].disciplines = e.value.clone(); }
				DevotionField::Name => { devotions[i].name = e.value.clone(); }
				DevotionField::Reference => { devotions[i].reference = e.value.clone(); }
			}
		}
		None =>
		{
			match prop
			{
				DevotionField::Action => { devotions.push(Devotion { action: e.value.clone(), ..Default::default() }); }
				DevotionField::Cost => { devotions.push(Devotion { cost: e.value.clone(), ..Default::default() }); }
				DevotionField::DicePool => { devotions.push(Devotion { dicePool: e.value.clone(), ..Default::default() }); }
				DevotionField::Disciplines => { devotions.push(Devotion { disciplines: e.value.clone(), ..Default::default() }); }
				DevotionField::Name => { devotions.push(Devotion { name: e.value.clone(), ..Default::default() }); }
				DevotionField::Reference => { devotions.push(Devotion { reference: e.value.clone(), ..Default::default() }); }
			}
		}
	}
}
