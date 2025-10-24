use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_memo,
	use_signal, Button, Element, GlobalSignal, Input, Props, Readable, Signal,
	Writable};
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use super::data::Merit;

#[component]
pub fn MeritElement(index: usize, signal: Signal<Vec<Merit>>) -> Element
{
	let merit = &signal()[index];
	let dotValue: Signal<StateValue> = use_signal(|| merit.dots.into());
	let mut name = use_signal(|| merit.name.to_owned());
	
	use_memo(move || {
		if let Some(merit) = signal.write().get_mut(index)
		{
			merit.dots = dotValue().one;
			merit.name = name();
		}
	});
	
	return rsx!(
		rect
		{
			cross_align: "center",
			direction: "horizontal",
			spacing: "5",
			width: "50%",
			
			Input
			{
				onchange: move |value| name.set(value),
				value: name().to_owned(),
				width: "70%",
			}
			
			StatefulTrack
			{
				mode: StatefulMode::CircleOne,
				value: dotValue,
			}
					
			Button
			{
				onpress: move |_| _ = signal.write().remove(index),
				label { "x" }
			}
		}
	);
}
