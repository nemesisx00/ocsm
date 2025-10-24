use freya::events::{Code, KeyboardData, Modifiers};
use freya::prelude::{dioxus_elements, fc_to_builder, rsx, Button, Element,
	Event, GlobalSignal, IntoDynNode, Readable, ScrollView, ThemeProvider};
use crate::data::AppState;
use crate::constants::{BackgroundColor, TextColor, Theme};
use crate::gamesystems::{CofdSheetList, Vtr2eSheetList};
use crate::menu::{loadHandler, saveHandler, MainMenu};

pub fn App() -> Element
{
	return rsx!(
		ThemeProvider
		{
			theme: Theme,
			
			rect
			{
				background: BackgroundColor,
				color: TextColor,
				direction: "vertical",
				height: "fill",
				padding: "15",
				spacing: "10",
				width: "fill",
				
				onglobalkeyup: move |e| saveKeyEventHandler(e),
				
				rect
				{
					direction: "horizontal",
					main_align: "start",
					spacing: "5",
					
					if !AppState().menuOpen
					{
						Button
						{
							onpress: move |_| AppState.write().menuOpen = true,
							label { "Main Menu" }
						}
					}
					
					if AppState().menuOpen
					{
						MainMenu {}
					}
					
					if AppState().activeId.is_some()
					{
						Button
						{
							onpress: move |_| AppState.write().activeId = None,
							
							label { "Back" }
						}
					}
				}
				
				match AppState().activeId
				{
					None => rsx!(
						ScrollView
						{
							height: "fill",
							width: "fill",
							
							rect
							{
								direction: "vertical",
								spacing: "15",
								
								CofdSheetList {}
								Vtr2eSheetList {}
							}
						}
					),
					
					Some(id) => rsx!(
						ScrollView
						{
							height: "fill",
							width: "fill",
							
							{AppState().sheets.generateElement(id)}
						}
					)
				}
			}
		}
	);
}

fn saveKeyEventHandler(event: Event<KeyboardData>)
{
	match event.modifiers
	{
		Modifiers::CONTROL => match event.code
		{
			Code::KeyO => loadHandler(),
			Code::KeyS => saveHandler(),
			_ => {},
		},
		
		_ => {},
	}
}
