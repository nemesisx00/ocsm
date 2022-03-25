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

const DefaultLastIndex: usize = 1000;

pub fn Disciplines(cx: Scope) -> Element
{
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let disciplines = disciplinesRef.read();
	
	let lastIndex = use_state(&cx, || DefaultLastIndex);
	let showRemove = lastIndex.get() < &disciplines.len();
	
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
				
				disciplines.iter().enumerate().map(|(i, d)| rsx!(cx, div
				{
					class: "entry row",
					oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); },
					prevent_default: "oncontextmenu",
					
					Dots { class: "row".to_string(), key: "{i}", label: d.name.clone(), max: 5, value: d.value, handler: dotsHandler, handlerKey: i }
				}))
				
				div
				{
					class: "entry row",
					
					select
					{
						onchange: move |e| selectHandler(e, &cx),
						oncontextmenu: move |e| { e.cancel_bubble(); },
						prevent_default: "oncontextmenu",
						
						option { value: "", selected: "true", "Add a Discipline" }
						optionNames.iter().enumerate().map(|(i, name)| rsx!(cx, option { option { key: "{i}", value: "{name}", "{name}" } }))
					}
				}
			}
			
			showRemove.then(|| rsx!
			{
				div { class: "removePopUpOverlay column" }
				
				div
				{
					class: "removePopUpWrapper column",
					
					div
					{
						class: "removePopUp column",
						
						div { class: "row", "Are you sure you want to remove this Discipline?" }
						div
						{
							class: "row",
							
							button { onclick: move |e| { e.cancel_bubble(); removeDisciplineClickHandler(&cx, *(lastIndex.get())); lastIndex.set(DefaultLastIndex); }, prevent_default: "onclick", "Remove" }
							button { onclick: move |e| { e.cancel_bubble(); lastIndex.set(DefaultLastIndex); }, prevent_default: "onclick", "Cancel" }
						}
					}
				}
			})
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

fn removeDisciplineClickHandler(cx: &Scope, index: usize)
{
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let mut disciplines = disciplinesRef.write();
	
	if index < disciplines.len()
	{
		disciplines.remove(index);
	}
}

fn selectHandler(e: FormEvent, cx: &Scope)
{
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let mut disciplines = disciplinesRef.write();
	
	match disciplines.iter().filter(|d| d.name == e.value.clone()).next()
	{
		Some(_) => {}
		None => { disciplines.push(Discipline { name: e.value.clone(), ..Default::default() }); }
	}
}

// -----

pub fn Devotions(cx: Scope) -> Element
{
	let devotionsRef = use_atom_ref(&cx, KindredDevotions);
	let devotions = devotionsRef.read();
	
	let lastIndex = use_state(&cx, || DefaultLastIndex);
	let showRemove = lastIndex.get() < &devotions.len();
	
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
					oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); },
					prevent_default: "oncontextmenu",
					
					div
					{
						class: "row",
						
						div { class: "label first", "Name:" }
						input { r#type: "text", value: "{dev.name}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Name),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); }, prevent_default: "oncontextmenu" }
						div { class: "label second", "Cost:" }
						input { r#type: "text", value: "{dev.cost}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Cost),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); }, prevent_default: "oncontextmenu" }
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Dice Pool:" }
						input { r#type: "text", value: "{dev.dicePool}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::DicePool),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); }, prevent_default: "oncontextmenu" }
						div { class: "label second", "Action:" }
						input { r#type: "text", value: "{dev.action}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Action),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); }, prevent_default: "oncontextmenu" }
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Requirements:" }
						input { r#type: "text", value: "{dev.disciplines}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Disciplines),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); }, prevent_default: "oncontextmenu" }
						div { class: "label second", "Reference:" }
						input { r#type: "text", value: "{dev.reference}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Reference),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); }, prevent_default: "oncontextmenu" }
					}
				}))
				
				div
				{
					class: "new entry row",
					input { r#type: "text", value: "", placeholder: "Enter a new Devotion Name", onchange: move |e| inputHandler(e, &cx, None, DevotionField::Name), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
			}
			
			showRemove.then(|| rsx!
			{
				div { class: "removePopUpOverlay column" }
				
				div
				{
					class: "removePopUpWrapper column",
					
					div
					{
						class: "removePopUp column",
						
						div { class: "row", "Are you sure you want to remove this Devotion?" }
						div
						{
							class: "row",
							
							button { onclick: move |e| { e.cancel_bubble(); removeDevotionClickHandler(&cx, *(lastIndex.get())); lastIndex.set(DefaultLastIndex); }, prevent_default: "onclick", "Remove" }
							button { onclick: move |e| { e.cancel_bubble(); lastIndex.set(DefaultLastIndex); }, prevent_default: "onclick", "Cancel" }
						}
					}
				}
			})
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

fn removeDevotionClickHandler(cx: &Scope, index: usize)
{
	let devotionsRef = use_atom_ref(&cx, KindredDevotions);
	let mut devotions = devotionsRef.write();
	
	if index < devotions.len()
	{
		devotions.remove(index);
	}
}
