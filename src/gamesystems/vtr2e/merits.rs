use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx,
	use_context, use_memo, use_signal, Button, Element, GlobalSignal,
	IntoDynNode, Readable, Signal, Writable};
use crate::gamesystems::Vtr2eSheet;
use crate::gamesystems::cofd::MeritElement;

#[component]
pub fn MeritListElement() -> Element
{
	let mut sheetData = use_context::<Signal<Vtr2eSheet>>();
	let mut merits = use_signal(|| sheetData().merits.clone());
	use_memo(move || sheetData.write().merits = merits());
	
	return rsx!(
		rect
		{
			cross_align: "center",
			direction: "vertical",
			margin: "15 0 0 0",
			spacing: "5",
			width: "fill",
			
			label
			{
				font_size: "20",
				margin: "0 0 5 0",
				text_align: "center",
				width: "fill",
				"Merits"
			}
			
			for i in 0..merits().len()
			{
				if i % 2 == 0
				{
					rect
					{
						direction: "horizontal",
						min_width: "504",
						spacing: "40",
						width: "80%",
						
						MeritElement { index: i, signal: merits }
						
						if i + 1 < merits().len()
						{
							MeritElement { index: i + 1, signal: merits }
						}
					}
				}
			}
			
			rect
			{
				direction: "horizontal",
				main_align: "center",
				margin: "5 0 0 0",
				width: "fill",
				
				Button
				{
					onpress: move |_| merits.write().push(Default::default()),
					label { "Add Merit" }
				}
			}
		}
	);
}
