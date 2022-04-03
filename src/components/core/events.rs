#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::MouseEvent,
	prelude::*,
};

pub fn showRemovePopUpWithIndex<'a>(event: MouseEvent,
	clickedX: &'a UseState<i32>,
	clickedY: &'a UseState<i32>,
	lastIndex: &'a UseState<usize>,
	showRemove: &'a UseState<bool>,
	index: usize)
{
	event.cancel_bubble();
	clickedX.set(event.data.client_x);
	clickedY.set(event.data.client_y);
	lastIndex.set(index);
	showRemove.set(true);
}

pub fn showRemovePopUp<'a>(event: MouseEvent,
	clickedX: &'a UseState<i32>,
	clickedY: &'a UseState<i32>,
	showRemove: &'a UseState<bool>)
{
	event.cancel_bubble();
	clickedX.set(event.data.client_x);
	clickedY.set(event.data.client_y);
	showRemove.set(true);
}

pub fn hideRemovePopUp<'a>(event: MouseEvent, showRemove: &'a UseState<bool>)
{
	event.cancel_bubble();
	showRemove.set(false);
}
