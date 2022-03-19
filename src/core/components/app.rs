#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::core::components::check::{
	CheckCircle,
	CheckLine,
	CheckLineState
};
use crate::wod::vampire::components::sheet::Sheet;

pub fn App(cx: Scope) -> Element
{
	return cx.render(rsx!
	{
		style { [include_str!("../../../static/app.css")] }
		
		div
		{
			class: "App",
			
			Sheet { }
		}
	});
}
