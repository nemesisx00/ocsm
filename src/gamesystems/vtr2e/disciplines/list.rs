use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx,
	use_context, use_memo, use_signal, Button, Element, GlobalSignal,
	IntoDynNode, Readable, Signal, Writable};
use itertools::Itertools;
use crate::gamesystems::{vtr2e::data::Discipline, Vtr2eSheet};
use super::entry::DisciplineElement;

#[component]
pub fn DisciplineListElement() -> Element
{
	let mut sheetData = use_context::<Signal<Vtr2eSheet>>();
	let mut disciplines = use_signal(|| sheetData().disciplines);
	
	use_memo(move || sheetData.write().disciplines = disciplines());
	
	let mut rows = vec![vec![]];
	let mut row = 0;
	for (i, (d, _)) in disciplines().iter().sorted().enumerate()
	{
		if i > 0 && i % 3 == 0
		{
			rows.push(vec![]);
			row = rows.len() - 1;
		}
		
		rows[row].push(d.to_owned());
	}
	
	return rsx!(
		rect
		{
			cross_align: "center",
			direction: "vertical",
			main_align: "space-evenly",
			margin: "15 0 0 0",
			spacing: "5",
			width: "fill",
			
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
					"Disciplines"
				}
				
				rect
				{
					direction: "horizontal",
					main_align: "center",
					
					Button
					{
						onpress: move |_| _ = disciplines.write().insert(Discipline::None, 0),
						label { "+" }
					}
				}
			}
			
			for row in rows
			{
				rect
				{
					cross_align: "center",
					direction: "horizontal",
					main_align: "space-between",
					spacing: "10",
					
					for d in row
					{
						DisciplineElement { discipline: d, signal: disciplines }
					}
				}
			}
		}
	);
}
