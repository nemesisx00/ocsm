use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx,
	use_context_provider, use_memo, use_signal, Element, GlobalSignal, Props,
	Readable};
use crate::data::{AppState, CharacterSheet, SheetId};
use crate::gamesystems::vtr2e::disciplines::list::DisciplineListElement;
use super::info::InfoElement;
use super::merits::MeritListElement;
use super::tracks::TracksElement;
use super::traits::TraitsElement;

#[component]
pub fn SheetElement(id: SheetId) -> Element
{
	let sheetData = use_signal(|| match AppState().sheets.vtr2e.iter()
		.find(|d| d.id == id)
	{
		None => Default::default(),
		Some(data) => data.to_owned(),
	});
	
	use_context_provider(|| sheetData);
	
	use_memo(move || {
		if let Some(sheet) = AppState.write().sheets.vtr2e.iter_mut()
			.find(|d| d.id == sheetData().id)
		{
			sheet.update(&sheetData());
		}
	});
	
	return rsx!(
		rect
		{
			direction: "vertical",
			margin: "0 0 400 0",
			spacing: "10",
			width: "fill",
			
			InfoElement {}
			TracksElement {}
			TraitsElement {}
			DisciplineListElement {}
			MeritListElement {}
		}
	);
}
