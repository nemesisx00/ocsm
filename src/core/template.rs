#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::Scope;

pub trait StatefulTemplate
{
	/// Pull the global state down into this template.
	fn pull<T>(&mut self, cx: &Scope<T>);
	
	/// Push this template up into the global state.
	fn push<T>(&mut self, cx: &Scope<T>);
	
	/// Detect and resolve any internal data errors.
	fn validate(&mut self);
}
