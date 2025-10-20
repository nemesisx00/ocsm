use std::sync::LazyLock;
use bytes::Bytes;
use freya::hooks::use_platform;
use freya::prelude::{component, dioxus_elements, rsx, static_bytes, CursorIcon, Element, GlobalSignal, Props, Readable, Signal, Writable};
use crate::components::{StatefulMode, StateValue};
use crate::io::*;

static CircleEmptyBytes: LazyLock<Bytes> = LazyLock::new(|| loadOrDefaultImage(&CircleEmpty.to_string()));
static CircleFullBytes: LazyLock<Bytes> = LazyLock::new(|| loadOrDefaultImage(&CircleFull.to_string()));
static CircleHalfBytes: LazyLock<Bytes> = LazyLock::new(|| loadOrDefaultImage(&CircleHalf.to_string()));
static CircleRedBytes: LazyLock<Bytes> = LazyLock::new(|| loadOrDefaultImage(&CircleRed.to_string()));
static BoxBorderBytes: LazyLock<Bytes> = LazyLock::new(|| loadOrDefaultImage(&BoxBorder.to_string()));
static SlashOneBytes: LazyLock<Bytes> = LazyLock::new(|| loadOrDefaultImage(&SlashOne.to_string()));
static SlashTwoBytes: LazyLock<Bytes> = LazyLock::new(|| loadOrDefaultImage(&SlashTwo.to_string()));
static SlashThreeBytes: LazyLock<Bytes> = LazyLock::new(|| loadOrDefaultImage(&SlashThree.to_string()));

const ImageDimension: &str = "16";

#[component]
pub fn StatefulButton(index: u32, mode: StatefulMode, state: Signal<StateValue>) -> Element
{
	let emptyBytes = match mode
	{
		StatefulMode::BoxOne => &BoxBorderBytes,
		StatefulMode::BoxTwo => &BoxBorderBytes,
		StatefulMode::BoxThree => &BoxBorderBytes,
		StatefulMode::CircleOne => &CircleEmptyBytes,
		StatefulMode::CircleThree => &CircleEmptyBytes,
	};
	let platform = use_platform();
	let stateValue = selectState(index, state());
	let bytes = selectImage(mode, stateValue);
	
	return rsx!(
		image
		{
			cross_align: "center",
			height: ImageDimension,
			image_data: static_bytes(emptyBytes),
			main_align: "center",
			margin: "0",
			width: ImageDimension,
			
			image
			{
				onpointerenter: move |_| platform.set_cursor(CursorIcon::Pointer),
				onpointerleave: move |_| platform.set_cursor(CursorIcon::Default),
				onpointerpress: move |_| state.set(updateStateValue(index, mode, state())),
				
				cross_align: "center",
				height: ImageDimension,
				image_data: match bytes {
					None => None,
					Some(b) => Some(static_bytes(b)),
				},
				main_align: "center",
				margin: "0",
				width: ImageDimension,
			}
		}
	);
}

fn loadOrDefaultImage(fileName: &String) -> Bytes
{
	return match loadImageToBytes(fileName)
	{
		Err(e) => {
			println!("Failed to read image {} bytes: {:?}", fileName, e);
			Default::default()
		},
		Ok(b) => b,
	};
}

fn processTwoStateLogic(index: u32, stateValue: StateValue) -> StateValue
{
	let indexValue = index + 1;
	let mut state = stateValue;
	
	match indexValue == state.two
	{
		false => match indexValue > state.two
		{
			false => state.two -= 1,
			true => match indexValue == state.two + state.one
			{
				false => match indexValue > state.two + state.one
				{
					false => {
						state.one -= 1;
						state.two += 1;
					},
					true => state.one += 1,
				},
				true => {
					state.one -= 1;
					state.two += 1;
				},
			},
		},
		true => state.two -= 1,
	}
	
	return state;
}

fn processThreeStateLogic(index: u32, stateValue: StateValue) -> StateValue
{
	let indexValue = index + 1;
	let mut state = stateValue;
	
	match indexValue == state.three
	{
		false => match indexValue > state.three
		{
			false => state.three -= 1,
			true => match indexValue == state.three + state.two
			{
				false => match indexValue > state.three + state.two
				{
					false => {
						state.two -= 1;
						state.three += 1;
					},
					true => match indexValue == state.three + state.two + state.one
					{
						false => match indexValue > state.three + state.two + state.one
						{
							false => {
								state.one -= 1;
								state.two += 1;
							},
							true => state.one += 1,
						},
						true => {
							state.one -= 1;
							state.two += 1;
						},
					},
				},
				true => {
					state.two -= 1;
					state.three += 1;
				},
			},
		},
		true => state.three -= 1,
	}
	
	return state;
}

fn selectImage(mode: StatefulMode, state: u32) -> Option<&'static LazyLock<Bytes>>
{
	return match mode
	{
		StatefulMode::CircleOne => match state
		{
			1 => Some(&CircleFullBytes),
			_ => None,
		},
		
		StatefulMode::CircleThree => match state
		{
			1 => Some(&CircleHalfBytes),
			2 => Some(&CircleFullBytes),
			3 => Some(&CircleRedBytes),
			_ => None,
		},
		
		StatefulMode::BoxOne => match state
		{
			1 => Some(&SlashTwoBytes),
			_ => None,
		},
		
		StatefulMode::BoxTwo => match state
		{
			1 => Some(&SlashOneBytes),
			2 => Some(&SlashTwoBytes),
			_ => None,
		},
		
		StatefulMode::BoxThree => match state
		{
			1 => Some(&SlashOneBytes),
			2 => Some(&SlashTwoBytes),
			3 => Some(&SlashThreeBytes),
			_ => None,
		},
	};
}

fn selectState(index: u32, state: StateValue) -> u32
{
	return match index < state.three
	{
		false => match index < state.three + state.two
		{
			false => match index < state.three + state.two + state.one
			{
				false => 0,
				true => 1,
			},
			true => 2,
		},
		true => 3,
	};
}

fn updateStateValue(index: u32, mode: StatefulMode, stateValue: StateValue) -> StateValue
{
	let indexValue = index + 1;
	let mut state = stateValue;
	
	match mode
	{
		StatefulMode::BoxOne => match indexValue == state.one
		{
			false => state.one = indexValue,
			true => state.one = index,
		},
		
		StatefulMode::BoxTwo => state = processTwoStateLogic(index, stateValue),
		StatefulMode::BoxThree => state = processThreeStateLogic(index, stateValue),
		
		StatefulMode::CircleOne => match indexValue == state.one
		{
			false => state.one = indexValue,
			true => state.one = index,
		},
		
		StatefulMode::CircleThree => state = processThreeStateLogic(index, stateValue)
	}
	
	return state;
}
