#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::MouseEvent,
	prelude::*
};

const SvgPathLineBack: &str = "M0,0 L12,12";
const SvgPathLineFwd: &str = "M12,0 L0,12";
const SvgPathLineVert: &str = "M6,0 L6,12";
const SvgPathEnd: &str = "Z";

pub fn getSingleCheckLineState(checked: bool) -> CheckLineState
{
	return match checked
	{
		true => CheckLineState::Double,
		false => CheckLineState::None,
	};
}

/// The properties struct for `CheckCircle`.
#[derive(Props)]
pub struct CheckCircleProps<'a>
{
	checked: bool,
	
	#[props(optional)]
	class: Option<String>,
	
	#[props(optional)]
	tooltip: Option<String>,
	
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
		
		let tooltipEq = self.tooltip == other.tooltip;
		
		return checkedEq && classEq && tooltipEq;
	}
}

/// Generate a clickable circle rendered by inline SVG.
pub fn CheckCircle<'a>(cx: Scope<'a, CheckCircleProps<'a>>) -> Element<'a>
{
	let classNames = match &cx.props.class
	{
		Some(class) => match cx.props.checked
		{
			true => format!(" {} {}", class, "checked"),
			false => format!(" {}", class),
		},
		None => match cx.props.checked
		{
			true => " checked".to_string(),
			false => "".to_string(),
		}
	};
	
	let tooltip = match &cx.props.tooltip
	{
		Some(tt) => tt.clone(),
		None => "".to_string(),
	};
	
	return cx.render(rsx!
	{
		div
		{
			class: "checker circle{classNames}",
			title: "{tooltip}",
			onclick: move |e|
			{
				e.cancel_bubble();
				match &cx.props.onclick
				{
					Some(handler) => handler.call(e),
					None => {}
				}
			},
			prevent_default: "oncontextmenu",
			
			svg
			{
				xmlns: "http://www.w3.org/20000/svg",
				"viewBox": "0 0 14 14",
				circle { cx: "7", cy: "7", r: "7" }
			}
		}
	});
}

/// The possible states of a `CheckLine`.
#[derive(Clone, Copy, PartialEq)]
pub enum CheckLineState
{
	None,
	Single,
	Double,
	Triple
}

///The properties struct for `CheckLine`.
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

/// Generate a clickable box rendered by inline SVG.
pub fn CheckLine<'a>(cx: Scope<'a, CheckLineProps<'a>>) -> Element<'a>
{
	let path = buildPath(cx.props.lineState);
	
	return cx.render(rsx!
	{
		div
		{
			class: "checker line",
			onclick: move |e|
			{
				e.cancel_bubble();
				match &cx.props.onclick
				{
					Some(handler) => handler.call(e),
					None => {}
				}
			},
			prevent_default: "oncontextmenu",
			
			svg
			{
				xmlns: "http://www.w3.org/20000/svg",
				"viewBox": "0 0 12 12",
				path { d: "{path}" }
			}
		}
	});
}

/// Construct an SVG Path based on the given `CheckLineState`.
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
