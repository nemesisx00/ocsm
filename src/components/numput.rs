use freya::prelude::{component, fc_to_builder, rsx, Element, GlobalSignal, Input,
	InputValidator, Props, Readable, Signal, Writable};

#[component]
pub fn NumericInput(value: Signal<u32>, max: Option<u32>, min: Option<u32>, width: Option<&'static str>) -> Element
{
	let max = match max
	{
		None => u32::MAX,
		Some(m) => m,
	};
	
	let min = match min
	{
		None => u32::MIN,
		Some(m) => m,
	};
	
	let width = match width
	{
		None => "auto",
		Some(w) => w,
	};
	
	return rsx!(
		Input
		{
			value: value().to_string(),
			width: "{width}",
			onchange: move |text: String| value.set(text.parse::<u32>().unwrap()),
			onvalidate: move |validator: InputValidator| validate(validator, max, min),
		}
	);
}

fn validate(validator: InputValidator, max: u32, min: u32)
{
	validator.set_valid(match validator.text().parse::<u32>()
	{
		Err(_) => false,
		Ok(num) => min <= num && num <= max,
	});
}
