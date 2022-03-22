#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::cod::{
	vtr2e::{
		components::{
			sheet::{
				Sheet,
			},
		},
	},
};

pub fn App(scope: Scope) -> Element
{
	return scope.render(rsx!
	{
		style { [include_str!("../static/app.css")] }
		
		div
		{
			class: "app column",
			
			Sheet { }
		}
	});
}
