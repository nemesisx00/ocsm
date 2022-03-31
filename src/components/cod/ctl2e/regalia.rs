#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*
};
use strum::IntoEnumIterator;
use crate::{
	cod::{
		ctl2e::{
			enums::{
				Regalia,
				Seeming,
			},
			state::ChangelingFavoredRegalia,
		},
		enums::CoreDetail,
		state::CharacterDetails,
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
	},
};

/// The UI Component handling a Changeling: The Lost 2e Changeling's Favored Regalia.
pub fn FavoredRegalia(cx: Scope) -> Element
{
	let detailsRef = use_atom_ref(&cx, CharacterDetails);
	let details = detailsRef.read();
	let favoredRegalia = use_read(&cx, ChangelingFavoredRegalia);
	let setFavoredRegalia = use_set(&cx, ChangelingFavoredRegalia);
	
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	
	let mut seemingRegalia = "".to_string();
	for s in Seeming::iter()
	{
		if s.as_ref().to_string() == details[&CoreDetail::TypePrimary]
		{
			seemingRegalia = Regalia::getBySeeming(s).as_ref().to_string();
		}
	}
	
	let mut optionNames = Vec::<String>::new();
	for r in Regalia::iter()
	{
		let name = r.as_ref().to_string();
		match name == seemingRegalia
		{
			true => {}
			false => { optionNames.push(name); }
		}
	}
	
	let chosenRegalia: String = match favoredRegalia
	{
		Some(fr) => { fr.as_ref().to_string() }
		None => { "".to_string() }
	};
	
	let showSelect = match chosenRegalia == "".to_string()
	{
		true => { true }
		false => { false }
	};
	
	let showRegalia = match chosenRegalia == "".to_string()
	{
		true => { false }
		false => { true }
	};
	
	return cx.render(rsx!
	{
		div
		{
			class: "regalia entryListWrapper column",
			
			div { class: "entryListLabel", "Favored Regalia" }
			
			div
			{
				class: "entryList row",
				div { class: "regalia", oncontextmenu: move |e| { e.cancel_bubble(); }, prevent_default: "oncontextmenu", "{seemingRegalia}" }
				
				showRegalia.then(|| rsx!
				{
					div
					{
						class: "regalia entry row",
						oncontextmenu: move |e|
						{
							e.cancel_bubble();
							clickedX.set(e.data.client_x);
							clickedY.set(e.data.client_y);
							showRemove.set(true);
						},
						prevent_default: "oncontextmenu",
						"{chosenRegalia}"
					}
				})
				
				showSelect.then(|| rsx!
				{
					div
					{
						class: "regalia entry row",
						
						select
						{
							onchange: move |e| selectHandler(e, &cx),
							oncontextmenu: move |e| { e.cancel_bubble(); },
							prevent_default: "oncontextmenu",
							
							option { value: "", selected: "true", "Add a Favored Regalia" }
							optionNames.iter().enumerate().map(|(i, name)| rsx!(cx, option { option { key: "{i}", value: "{name}", "{name}" } }))
						}
					}
				})
			}
			
			showRemove.then(|| rsx!
			{
				div
				{
					class: "removePopUpWrapper column",
					style: "left: {posX}px; top: {posY}px;",
					onclick: move |e| { e.cancel_bubble(); showRemove.set(false); },
					prevent_default: "onclick",
					
					div
					{
						class: "removePopUp column",
						
						div { class: "row", "Are you sure you want to remove this Favored Regalia?" }
						div
						{
							class: "row",
							
							button { onclick: move |e| { e.cancel_bubble(); setFavoredRegalia(None); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
							button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
						}
					}
				}
			})
		}
	});
}

/// Event handler triggered when the "Add a Favored Regalia" select input's value changes.
fn selectHandler(e: FormEvent, cx: &Scope)
{
	let favoredRegalia = use_read(&cx, ChangelingFavoredRegalia);
	let setFavoredRegalia = use_set(&cx, ChangelingFavoredRegalia);
	
	let regalia = e.value.to_string();
	
	match Regalia::asMap().iter().filter(|(_, name)| *name == &regalia).next()
	{
		Some((r, _)) =>
		{
			match favoredRegalia
			{
				Some(_) => {}
				None => { setFavoredRegalia(Some(*r)); }
			}
		}
		None => {}
	}
}
