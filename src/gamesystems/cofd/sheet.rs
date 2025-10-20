use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx,
	use_context_provider, use_signal, Element, GlobalSignal, Props, Readable};
use crate::data::{AppState, SheetId};
use crate::gamesystems::cofd::info::InfoElement;
use crate::gamesystems::cofd::tracks::TracksElement;
use crate::gamesystems::cofd::traits::TraitsElement;

#[component]
pub fn SheetElement(id: SheetId) -> Element
{
	let sheetData = use_signal(|| match AppState().sheets.cofd.iter()
		.find(|d| d.id == id)
	{
		None => Default::default(),
		Some(data) => data.to_owned(),
		
	});
	
	use_context_provider(|| sheetData);
	
	return rsx!(
		rect
		{
			direction: "vertical",
			spacing: "10",
			
			InfoElement {}
			TracksElement {}
			TraitsElement {}
		}
	);
}
