use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_context,
	use_memo, use_signal, Accordion, AccordionBody, AccordionSummary,
	Button, Element, GlobalSignal, Input, IntoDynNode, Readable, Signal,
	Writable};
use crate::gamesystems::Vtr2eSheet;

/**
Component enabling display and editing of general free text character information
typically displayed at the top of a character sheet, such as character concept,
name, etc.
*/
#[component]
pub fn ConditionsElement() -> Element
{
	let mut sheetData = use_context::<Signal<Vtr2eSheet>>();
	
	let mut aspirations = use_signal(|| sheetData().aspirations);
	let mut conditions = use_signal(|| sheetData().conditions);
	
	if aspirations().is_empty()
	{
		aspirations.write().push(String::default());
	}
	
	if conditions().is_empty()
	{
		conditions.write().push(String::default());
	}
	
	use_memo(move || {
		sheetData.write().aspirations = aspirations();
		sheetData.write().conditions = conditions();
	});
	
	return rsx!(
		rect
		{
			cross_align: "center",
			direction: "vertical",
			width: "fill",
			
			rect
			{
				cross_align: "center",
				direction: "vertical",
				width: "85%",
				
				Accordion
				{
					summary: rsx!(AccordionSummary
					{
						rect
						{
							cross_align: "center",
							direction: "horizontal",
							main_align: "center",
							spacing: "15",
							width: "fill",
							
							label
							{
								text_align: "center",
								width: "49%",
								"Aspirations"
							}
							
							label
							{
								text_align: "center",
								width: "49%",
								"Conditions"
							}
						}
					}),
					
					AccordionBody
					{
						rect
						{
							cross_align: "center",
							direction: "horizontal",
							main_align: "center",
							spacing: "15",
							width: "fill",
							
							rect
							{
								direction: "vertical",
								spacing: "5",
								width: "49%",
								
								for (i, aspiration) in aspirations().iter().enumerate()
								{
									Input
									{
										onchange: move |value| aspirations.write()[i] = value,
										value: aspiration,
										width: "fill",
									}
								}
								
								rect
								{
									direction: "horizontal",
									main_align: "center",
									spacing: "15",
									width: "fill",
									
									Button
									{
										onpress: move |_| _ = aspirations.write().push(String::default()),
										label { "+" }
									}
									
									Button
									{
										onpress: move |_| if aspirations().len() > 1
										{
											_ = aspirations.write().pop();
										},
										label { "-" }
									}
								}
							}
							
							rect
							{
								direction: "vertical",
								spacing: "5",
								width: "49%",
								
								for (i, condition) in conditions().iter().enumerate()
								{
									Input
									{
										onchange: move |value| conditions.write()[i] = value,
										value: condition,
										width: "fill",
									}
								}
								
								rect
								{
									direction: "horizontal",
									main_align: "center",
									spacing: "15",
									width: "fill",
									
									Button
									{
										onpress: move |_| _ = conditions.write().push(String::default()),
										label { "+" }
									}
									
									Button
									{
										onpress: move |_| if conditions().len() > 1
										{
											_ = conditions.write().pop();
										},
										label { "-" }
									}
								}
							}
						}
					}
				}
			}
		}
	);
}
