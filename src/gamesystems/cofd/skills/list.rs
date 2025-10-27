use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, Button,
	Element, GlobalSignal, IntoDynNode, Props, Readable, Signal, Writable};
use itertools::Itertools;
use super::SkillSpecialtyElement;
use crate::gamesystems::cofd::data::Specialty;

#[component]
pub fn SkillSpecialtyListElement(
	signal: Signal<Vec<Specialty>>,
	width: Option<&'static str>
) -> Element
{
	let width = match width
	{
		None => "auto",
		Some(w) => w,
	};
	
	let mut rows = vec![];
	let mut row = 0;
	
	for (i, s) in signal().iter()
		.sorted()
		.cloned()
		.enumerate()
	{
		if i % 2 == 0
		{
			rows.push(vec![]);
			row = rows.len() - 1;
		}
		
		rows[row].push(s);
	}
	
	return rsx!(
		rect
		{
			cross_align: "center",
			direction: "vertical",
			main_align: "center",
			margin: "15 0 0 0",
			spacing: "5",
			width: "{width}",
			
			rect
			{
				direction: "horizontal",
				main_align: "center",
				margin: "0 0 5 0",
				spacing: "5",
				width: "fill",
				
				label
				{
					font_size: "20",
					text_align: "center",
					"Skill Specialties"
				}
				
				rect
				{
					direction: "horizontal",
					main_align: "center",
					
					Button
					{
						onpress: move |_| _ = signal.write().push(Default::default()),
						label { "+" }
					}
				}
			}
			
			rect
			{
				direction: "vertical",
				spacing: "5",
				width: "99%",
				
				for row in rows
				{
					rect
					{
						content: "flex",
						direction: "horizontal",
						spacing: "15",
						width: "fill",
						
						for s in row
						{
							SkillSpecialtyElement
							{
								specialty: s,
								signal: signal,
								width: "flex"
							}
						}
					}
				}
			}
		}
	);
}
