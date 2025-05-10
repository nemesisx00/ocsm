use std::cell::Cell;
use std::sync::OnceLock;
use gtk4::glib::subclass::Signal;
use gtk4::Grid;
use gtk4::glib::{self, closure_local, Properties};
use gtk4::prelude::*;
use gtk4::subclass::prelude::*;
use crate::button::stateful::{Signal_StateToggled, StatefulButton, StatefulMode};
use super::data::StateValue;

#[derive(Default, Properties)]
#[properties(wrapper_type = super::StatefulTrack)]
pub struct StatefulTrack
{
	#[property(construct, default = 5, get, set = Self::setMaximum)]
	maximum: Cell<u32>,
	
	#[property(construct, default = StatefulMode::CircleOne.into(), get, set)]
	mode: Cell<u32>,
	
	#[property(construct, default = 10, get, set = Self::setRowLength)]
	rowLength: Cell<u32>,
	
	#[property(construct, default = false, get, set)]
	withSpace: Cell<bool>,
	
	value: Cell<StateValue>,
}

impl GridImpl for StatefulTrack {}

#[glib::derived_properties]
impl ObjectImpl for StatefulTrack
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.obj().add_css_class("statefulTrack");
		
		if self.withSpace.get() == true
		{
			self.obj().add_css_class("withSpace");
		}
		
		self.refresh();
	}
	
	fn dispose(&self)
	{
		self.clear();
	}
	
	fn signals() -> &'static [Signal]
	{
		static SIGNALS: OnceLock<Vec<Signal>> = OnceLock::new();
		return SIGNALS.get_or_init(|| {
			vec![
				Signal::builder(super::StatefulTrack::Signal_ValueUpdated)
					.param_types([
						u32::static_type(),
						u32::static_type(),
						u32::static_type(),
					])
					.build(),
			]
		});
	}
}

#[glib::object_subclass]
impl ObjectSubclass for StatefulTrack
{
	const NAME: &'static str = "StatefulTrack";
	type ParentType = Grid;
	type Type = super::StatefulTrack;
}

impl WidgetImpl for StatefulTrack {}

impl StatefulTrack
{
	fn clear(&self)
	{
		let obj = self.obj();
		while let Some(child) = obj.last_child()
		{
			obj.remove(&child);
		}
	}
	
	pub fn refresh(&self)
	{
		self.clear();
		
		let value = self.value.get();
		for i in 0..self.maximum.get()
		{
			let mut state = 0;
			
			if i < value.three + value.two + value.one
			{
				state = 1;
			}
			
			if i < value.three + value.two
			{
				state = 2;
			}
			
			if i < value.three
			{
				state = 3;
			};
			
			let button = StatefulButton::withState(self.mode.get(), state);
			button.set_index(i);
			
			let me = self;
			button.connect_closure(
				Signal_StateToggled,
				false,
				closure_local!(#[weak] me, move |_: StatefulButton, index: u32| {
					match me.mode.get().into()
					{
						StatefulMode::CircleOne | StatefulMode::BoxOne => {
							let indexValue = match me.value.get().one == index + 1
							{
								true => index,
								false => index + 1,
							};
							
							me.setValue(StateValue {
								one: indexValue,
								..Default::default()
							});
							
							me.refresh();
							me.updateValue();
						},
						
						/*
						If it's not one of the single-state modes, then we
						need to update the value by reading from the current
						state of each button before refreshing.
						*/
						_ => {
							me.updateValue();
							me.refresh();
						}
					}
				})
			);
			
			let rowLength = match self.rowLength.get() > 0
			{
				true => self.rowLength.get(),
				false => 10,
			};
			
			self.obj().attach(
				&button,
				(i % rowLength) as i32,
				(i / rowLength) as i32,
				1,
				1
			);
		}
	}
	
	pub fn getValue(&self) -> StateValue
	{
		return match self.obj().first_child().and_downcast::<StatefulButton>()
		{
			None => StateValue::default(),
			Some(first) => self.accumulateChildState(first, StateValue::default()),
		};
	}
	
	pub fn setMaximum(&self, max: u32)
	{
		self.maximum.set(max);
		
		let value = self.getValue();
		let truncatedValue = StateValue
		{
			one: match value.one > max
			{
				true => max,
				false => value.one,
			},
			
			two: match value.two > max
			{
				true => max,
				false => value.two,
			},
			
			three: match value.three > max
			{
				true => max,
				false => value.three,
			},
		};
		self.setValue(truncatedValue);
		
		self.refresh();
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.rowLength.set(length);
		self.refresh();
	}
	
	pub fn setValue(&self, value: StateValue)
	{
		self.value.set(value);
		self.refresh();
	}
	
	pub fn updateValue(&self)
	{
		let value = match self.obj().first_child().and_downcast::<StatefulButton>()
		{
			None => StateValue::default(),
			Some(first) => self.accumulateChildState(first, StateValue::default()),
		};
		
		self.value.set(value);
		
		self.obj().emit_by_name::<()>(
			super::StatefulTrack::Signal_ValueUpdated,
			&[&value.one, &value.two, &value.three]
		);
	}
	
	/**
	 Recurse through all child widgets and accumulate the StatefulButton::state()
	 values.
	 */
	fn accumulateChildState(&self, child: StatefulButton, acc: StateValue) -> StateValue
	{
		let mut val = acc;
		
		match child.state()
		{
			1 => val.one = val.one + 1,
			2 => val.two = val.two + 1,
			3 => val.three = val.three + 1,
			_ => {},
		}
		
		return match child.next_sibling().and_downcast::<StatefulButton>()
		{
			None => val,
			Some(next) => self.accumulateChildState(next, val),
		};
	}
}
