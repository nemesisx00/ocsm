use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx,
	use_context, use_memo, use_signal, Button, Element, GlobalSignal,
	IntoDynNode, Props, Readable, Signal, Writable};
use itertools::Itertools;
use crate::gamesystems::Vtr2eSheet;
use crate::gamesystems::vtr2e::data::Discipline;
use super::entry::DisciplineElement;

#[component]
pub fn DisciplineListElement(width: Option<&'static str>) -> Element
{
	let width = match width
	{
		None => "auto",
		Some(w) => w,
	};
	
	let mut sheetData = use_context::<Signal<Vtr2eSheet>>();
	let mut disciplines = use_signal(|| sheetData().disciplines);
	
	use_memo(move || sheetData.write().disciplines = disciplines());
	
	let mut rows = vec![];
	let mut row = 0;
	
	for (i, (d, _)) in disciplines().iter()
		.sorted()
		.enumerate()
	{
		if i % 3 == 0
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
					content: "flex",
					direction: "horizontal",
					main_align: "space-around",
					spacing: "5",
					width: "fill",
					
					for d in row
					{
						DisciplineElement
						{
							discipline: d,
							signal: disciplines,
							width: "flex"
						}
					}
				}
			}
		}
	);
}
