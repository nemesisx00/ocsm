use std::collections::HashMap;
use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_memo,
	use_signal, Button, Dropdown, DropdownItem, Element, GlobalSignal,
	IntoDynNode, Props, Readable, ScrollView, Signal, Writable};
use strum::IntoEnumIterator;
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use super::super::data::Discipline;

#[component]
pub fn DisciplineElement(
	discipline: Discipline,
	signal: Signal<HashMap<Discipline, u32>>,
	width: Option<&'static str>
) -> Element
{
	let width = match width
	{
		None => "auto",
		Some(w) => w,
	};
	
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
	
	use_memo(move || if let Some(dots) = signal.write().get_mut(&selected())
	{
		*dots = dotValue().one;
	});
	
	return rsx!(
		rect
		{
			cross_align: "center",
			direction: "horizontal",
			main_align: "center",
			spacing: "5",
			width: "{width}",
			
			Dropdown
			{
				selected_item: rsx!(label { "{selected().as_ref()}" }),
				
				ScrollView
				{
					height: "200",
					
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
			}
			
			StatefulTrack
			{
				mode: StatefulMode::CircleOne,
				value: dotValue,
			}
					
			Button
			{
				onpress: move |_| _ = signal.write().remove(&selected()),
				label { "x" }
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
