use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_signal,
	Element, GlobalSignal, IntoDynNode, Props, Readable, Signal, Writable};
use crate::components::{StatefulButton, StatefulMode, StateValue};

#[component]
pub fn StatefulTrack(
	mode: StatefulMode,
	max: Option<u32>,
	min: Option<u32>,
	rowMax: Option<u32>,
	value: Option<Signal<StateValue>>,
) -> Element
{
	let max = match max
	{
		None => 5,
		Some(m) => m,
	};
	
	let min = match min
	{
		None => 0,
		Some(m) => m,
	};
	
	let rowMax = match rowMax
	{
		None => 10,
		Some(rm) => rm,
	};
	
	let mut state = match value
	{
		None => use_signal(|| StateValue::default()),
		Some(v) => v
	};
	
	state.write().truncateMin(max, min);
	
	let mut rows = vec![vec![]];
	let mut r = 0;
	
	for i in 0..max
	{
		if (i % rowMax) == 0
		{
			rows.push(vec![]);
			r += 1;
		}
		
		rows[r].push(i as u32);
	}
	
	return rsx!(
		rect
		{
			direction: "vertical",
			main_align: "center",
			margin: "0 0 2 0",
			spacing: "5",
			
			for row in rows
			{
				rect
				{
					cross_align: "center",
					direction: "horizontal",
					margin: "0",
					spacing: "2",
					
					for i in row
					{
						if i > 0 && i % 5 == 0 && i % rowMax != 0
						{
							rect { margin: "0 6", }
						}
						
						StatefulButton
						{
							index: i,
							mode,
							state,
						}
					}
				}
			}
		}
	);
}
