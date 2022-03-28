#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	prelude::*,
	events::FormEvent
};
use strum::IntoEnumIterator;
use crate::{
	cod::{
		ctl2e::{
			details::{
				DetailType,
				Seeming,
			},
			regalia::{
				Regalia,
				Contract,
				ContractField,
			},
			state::{
				ChangelingContracts,
				ChangelingDetails,
				ChangelingFavoredRegalia,
			},
		},
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
	},
};

pub fn FavoredRegalia(cx: Scope) -> Element
{
	let detailsRef = use_atom_ref(&cx, ChangelingDetails);
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
		if s.as_ref().to_string() == details[&DetailType::Seeming]
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

// -----

pub fn Contracts(cx: Scope) -> Element
{
	let contractsRef = use_atom_ref(&cx, ChangelingContracts);
	let contracts = contractsRef.read();
	
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	
	return cx.render(rsx!
	{
		div
		{
			class: "contracts entryListWrapper column",
			
			div { class: "entryListLabel", "Contracts" }
			
			div
			{
				class: "entryList column",
				
				contracts.iter().enumerate().map(|(i, con)| rsx!(cx, div
				{
					class: "entry column",
					oncontextmenu: move |e|
					{
						e.cancel_bubble();
						clickedX.set(e.data.client_x);
						clickedY.set(e.data.client_y);
						lastIndex.set(i);
						showRemove.set(true);
					},
					prevent_default: "oncontextmenu",
					
					div
					{
						class: "row",
						oncontextmenu: move |e|
						{
							e.cancel_bubble();
							clickedX.set(e.data.client_x);
							clickedY.set(e.data.client_y);
							lastIndex.set(i);
							showRemove.set(true);
						},
						prevent_default: "oncontextmenu",
						
						div { class: "label first", "Name:" }
						input
						{
							r#type: "text",
							value: "{con.name}",
							onchange: move |e| inputHandler(e, &cx, Some(i), ContractField::Name),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu"
						}
						div { class: "label second", "Cost:" }
						input
						{
							r#type: "text",
							value: "{con.cost}",
							onchange: move |e| inputHandler(e, &cx, Some(i), ContractField::Cost),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu"
						}
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Dice Pool:" }
						input
						{
							r#type: "text",
							value: "{con.dicePool}",
							onchange: move |e| inputHandler(e, &cx, Some(i), ContractField::DicePool),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu"
						}
						div { class: "label second", "Action:" }
						input
						{
							r#type: "text",
							value: "{con.action}",
							onchange: move |e| inputHandler(e, &cx, Some(i), ContractField::Action),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu"
						}
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Regalia:" }
						input
						{
							r#type: "text",
							value: "{con.regalia}",
							onchange: move |e| inputHandler(e, &cx, Some(i), ContractField::Regalia),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu"
						}
						div { class: "label second", "Duration:" }
						input
						{
							r#type: "text",
							value: "{con.duration}",
							onchange: move |e| inputHandler(e, &cx, Some(i), ContractField::Duration),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu"
						}
					}
				}))
				
				div
				{
					class: "new entry row",
					input { r#type: "text", value: "", placeholder: "Enter a new Name", onchange: move |e| inputHandler(e, &cx, None, ContractField::Name), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
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
						
						div { class: "row", "Are you sure you want to remove this Contract?" }
						div
						{
							class: "row",
							
							button { onclick: move |e| { e.cancel_bubble(); removeContractClickHandler(&cx, *(lastIndex.get())); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
							button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
						}
					}
				}
			})
		}
	});
}

fn inputHandler(e: FormEvent, cx: &Scope, index: Option<usize>, prop: ContractField)
{
	let contractsRef = use_atom_ref(&cx, ChangelingContracts);
	let mut contracts = contractsRef.write();
	
	match index
	{
		Some(i) =>
		{
			match prop
			{
				ContractField::Action => { contracts[i].action = e.value.clone(); }
				ContractField::Cost => { contracts[i].cost = e.value.clone(); }
				ContractField::DicePool => { contracts[i].dicePool = e.value.clone(); }
				ContractField::Regalia => { contracts[i].regalia = e.value.clone(); }
				ContractField::Duration => { contracts[i].duration = e.value.clone(); }
				ContractField::Name => { contracts[i].name = e.value.clone(); }
			}
		}
		None =>
		{
			match prop
			{
				ContractField::Action => { contracts.push(Contract { action: e.value.clone(), ..Default::default() }); }
				ContractField::Cost => { contracts.push(Contract { cost: e.value.clone(), ..Default::default() }); }
				ContractField::DicePool => { contracts.push(Contract { dicePool: e.value.clone(), ..Default::default() }); }
				ContractField::Regalia => { contracts.push(Contract { regalia: e.value.clone(), ..Default::default() }); }
				ContractField::Duration => { contracts.push(Contract { duration: e.value.clone(), ..Default::default() }); }
				ContractField::Name => { contracts.push(Contract { name: e.value.clone(), ..Default::default() }); }
			}
		}
	}
}

fn removeContractClickHandler(cx: &Scope, index: usize)
{
	let contractsRef = use_atom_ref(&cx, ChangelingContracts);
	let mut contracts = contractsRef.write();
	
	if index < contracts.len()
	{
		contracts.remove(index);
	}
}
