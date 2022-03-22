#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use dioxus::events::MouseEvent;

const SvgPathLineBack: &str = "M0,0 L12,12";
const SvgPathLineFwd: &str = "M12,0 L0,12";
const SvgPathLineVert: &str = "M6,0 L6,12";
const SvgPathEnd: &str = "Z";

///
#[derive(Props)]
pub struct CheckCircleProps<'a>
{
	checked: bool,
	
	#[props(optional)]
	class: Option<String>,
	
	#[props(optional)]
	onclick: Option<EventHandler<'a, MouseEvent>>
}

impl<'a> PartialEq for CheckCircleProps<'a>
{
	fn eq(&self, other: &Self) -> bool
	{
		let checkedEq = self.checked == other.checked;
		
		let classEq = match &self.class
		{
			Some(cl1) => {
				match &other.class
				{
					Some(cl2) => cl1 == cl2,
					None => false
				}
			},
			None => {
				match &other.class
				{
					Some(_) => false,
					None => true
				}
			}
		};
		
		return checkedEq && classEq;
	}
}

/// Generate clickable a circle with inline SVG.
pub fn CheckCircle<'a>(scope: Scope<'a, CheckCircleProps<'a>>) -> Element<'a>
{
	let checkedClass = match scope.props.checked
	{
		true => " checked",
		false => ""
	};
	
	return scope.render(rsx!
	{
		div
		{
			class: "checker circle{checkedClass}",
			prevent_default: "onclick",
			onclick: move |e| {
				e.cancel_bubble();
				match &scope.props.onclick
				{
					Some(handler) => handler.call(e),
					None => {}
				}
			},
			
			svg
			{
				xmlns: "http://www.w3.org/20000/svg",
				"viewBox": "0 0 14 14",
				circle { cx: "7", cy: "7", r: "7" }
			}
		}
	});
}

/// Enumeration to designate how many line(s) should be drawn in a CheckLine.
#[derive(Clone, Copy, PartialEq)]
pub enum CheckLineState
{
	None,
	Single,
	Double,
	Triple
}

///
#[derive(Props)]
pub struct CheckLineProps<'a>
{
	lineState: CheckLineState,
	
	#[props(optional)]
	onclick: Option<EventHandler<'a, MouseEvent>>
}

impl<'a> PartialEq for CheckLineProps<'a>
{
	fn eq(&self, other: &Self) -> bool
	{
		let lineStateEq = self.lineState == other.lineState;
		
		return lineStateEq;
	}
}

/// Generate clickable a box with inline SVG.
pub fn CheckLine<'a>(scope: Scope<'a, CheckLineProps<'a>>) -> Element<'a>
{
	let path = buildPath(scope.props.lineState);
	
	return scope.render(rsx!
	{
		div
		{
			class: "checker line",
			prevent_default: "onclick",
			onclick: move |e| {
				e.cancel_bubble();
				match &scope.props.onclick
				{
					Some(handler) => handler.call(e),
					None => {}
				}
			},
			
			svg
			{
				xmlns: "http://www.w3.org/20000/svg",
				"viewBox": "0 0 12 12",
				path { d: "{path}" }
			}
		}
	});
}

/// Construct an SVG Path as a String based on the given CheckLineState.
fn buildPath(lineState: CheckLineState) -> String
{
	return match lineState
	{
		CheckLineState::None => String::default(),
		CheckLineState::Single => format!("{} {}", SvgPathLineBack, SvgPathEnd),
		CheckLineState::Double => format!("{} {} {}", SvgPathLineBack, SvgPathLineFwd, SvgPathEnd),
		CheckLineState::Triple => format!("{} {} {} {}", SvgPathLineBack, SvgPathLineFwd, SvgPathLineVert, SvgPathEnd)
	};
}
