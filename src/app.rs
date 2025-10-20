use dioxus_core::Event;
use freya::events::{Code, KeyboardData, Modifiers};
use freya::prelude::{dioxus_elements, fc_to_builder, rsx, Button, Element,
	GlobalSignal, IntoDynNode, Readable, ScrollView, Tab, Tabsbar,
	ThemeProvider};
use crate::data::AppState;
use crate::constants::{BackgroundColor, TextColor, Theme};
use crate::menu::{saveHandler, MainMenu};

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
						rect
						{
							direction: "vertical",
							spacing: "15",
							
							Tabsbar
							{
								for id in AppState().sheets.cofd.iter()
									.map(|s| s.id)
								{
									Tab
									{
										onpress: move |_| AppState.write().activeId = Some(id),
										
										label { "Mortal {id.index}" },
									}
								}
							}
							
							Tabsbar
							{
								for id in AppState().sheets.vtr2e.iter()
									.map(|s| s.id)
								{
									Tab
									{
										onpress: move |_| AppState.write().activeId = Some(id),
										
										label { "Vampire {id.index}" },
									}
								}
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
	match event.code
	{
		Code::KeyS => match event.modifiers
		{
			Modifiers::CONTROL => saveHandler(),
			
			_ => {},
		},
		
		_ => {},
	}
}
