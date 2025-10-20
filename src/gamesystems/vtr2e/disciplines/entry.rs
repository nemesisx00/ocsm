use std::collections::HashMap;
use freya::hooks::use_platform;
use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_memo, use_signal, CursorIcon, Dropdown, DropdownItem, Element, GlobalSignal, IntoDynNode, Props, Readable, Signal, Writable};
use strum::IntoEnumIterator;
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use super::super::data::Discipline;

#[component]
pub fn DisciplineElement(discipline: Discipline, signal: Signal<HashMap<Discipline, u32>>) -> Element
{
	let platform = use_platform();
	let dotValue: Signal<StateValue> = use_signal(|| match signal().get(&discipline)
	{
		None => Default::default(),
		Some(dots) => dots.to_owned().into(),
	});
	
	let mut selected = use_signal(|| match Discipline::iter()
		.find(|d| d == &discipline)
	{
		None => Default::default(),
		Some(d) => d,
	});
	
	use_memo(move || {
		if let Some(dots) = signal.write().get_mut(&selected())
		{
			*dots = dotValue().one;
		}
	});
	
	return rsx!(
		rect
		{
			cross_align: "center",
			direction: "horizontal",
			spacing: "5",
			width: "30%",
			
			Dropdown
			{
				selected_item: rsx!(label { "{selected().as_ref()}" }),
				
				for d in Discipline::iter()
					.filter(|d| !signal().contains_key(&d))
				{
					DropdownItem
					{
						selected: selected() == d,
						onpress: move |_| swapSelected(d, &mut selected, &mut signal),
						label { min_width: "100", "{d.as_ref()}" }
					}
				}
			}
			
			StatefulTrack
			{
				mode: StatefulMode::CircleOne,
				value: dotValue,
			}
			
			rect { margin: "0 0 0 5", }
			
			label
			{
				onpointerenter: move |_| platform.set_cursor(CursorIcon::Pointer),
				onpointerleave: move |_| platform.set_cursor(CursorIcon::Default),
				onclick: move |_| _ = signal.write().remove(&selected()),
				"X"
			}
		}
	);
}

fn swapSelected(
	discipline: Discipline,
	selected: &mut Signal<Discipline>,
	signal: &mut Signal<HashMap<Discipline, u32>>
)
{
	let dots = match signal().get(&selected())
	{
		None => 0,
		Some(d) => *d,
	};
	
	if signal().contains_key(&selected())
	{
		signal.write().remove(&selected());
	}
	
	selected.set(discipline);
	
	if selected() != Discipline::None && !signal().contains_key(&selected())
	{
		signal.write().insert(selected(), dots);
	}
}
