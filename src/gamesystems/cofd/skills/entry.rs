use dioxus::hooks::to_owned;
use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_memo,
	use_signal, Button, Dropdown, DropdownItem, Element, GlobalSignal, Input,
	IntoDynNode, Props, Readable, ScrollView, Signal, Writable};
use strum::IntoEnumIterator;
use crate::gamesystems::cofd::data::{Skill, Specialty};

#[component]
pub fn SkillSpecialtyElement(
	specialty: Specialty,
	signal: Signal<Vec<Specialty>>,
	width: Option<&'static str>
) -> Element
{
	let width = match width
	{
		None => "auto",
		Some(w) => w,
	};
	
	let sp = specialty.clone();
	
	let mut skill = use_signal(|| specialty.skill);
	let mut spec = use_signal(|| specialty.specialty);
	
	use_memo({
		to_owned![sp];
		move || if let Some(specialty) = signal.write().iter_mut()
			.find(|s| s == &&sp)
		{
			specialty.skill = skill();
			specialty.specialty = spec();
		}
	});
	
	return rsx!(
		rect
		{
			content: "flex",
			direction: "horizontal",
			spacing: "5",
			width: "{width}",
			
			Dropdown
			{
				selected_item: rsx!(label { width: "100", "{skill().as_ref()}" }),
				
				ScrollView
				{
					height: "200",
					width: "125",
					
					for s in Skill::iter()
					{
						DropdownItem
						{
							selected: skill() == s,
							onpress: move |_| skill.set(s),
							label { width: "100", "{s.as_ref()}" }
						}
					}
				}
			}
			
			Input
			{
				onchange: move |text| spec.set(text),
				value: spec(),
				width: "flex",
			}
					
			Button
			{
				onpress: {
					to_owned![sp];
					move |_| {
						if let Some(index) = signal().iter()
							.cloned()
							.enumerate()
							.find(|(_, s)| s.to_owned() == sp)
							.map(|(i, _)| i)
						{
							_ = signal.write().remove(index);
						}
					}
				},
				label { "x" }
			}
		}
	);
}
