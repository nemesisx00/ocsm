use std::cell::Cell;
use gtk4::Box;
use gtk4::glib::{self, closure_local, Properties};
use gtk4::prelude::*;
use gtk4::subclass::prelude::*;
use crate::button::stateful::{StatefulButton, StatefulMode};
use super::data::StateValue;

#[derive(Default, Properties)]
#[properties(wrapper_type = super::StatefulTrack)]
pub struct StatefulTrack
{
	#[property(construct, default = 5, get, set = StatefulTrack::setMax)]
	max: Cell<u32>,
	
	#[property(construct, default = StatefulMode::CircleOne.into(), get, set)]
	mode: Cell<u32>,
	
	#[property(construct, default = true, get, set)]
	withSpace: Cell<bool>,
	
	value: Cell<StateValue>,
}

impl BoxImpl for StatefulTrack {}

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
}

#[glib::object_subclass]
impl ObjectSubclass for StatefulTrack
{
	const NAME: &'static str = "StatefulTrack";
	type ParentType = Box;
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
	
	fn refresh(&self)
	{
		self.clear();
		
		let value = self.value.get();
		for i in 0..self.max.get()
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
			
			let but = StatefulButton::withState(self.mode.get(), state);
			let me = self;
			but.connect_closure("stateToggled", false, closure_local!(#[weak] me, move |_: StatefulButton| {
				me.updateValue();
				me.refresh();
			}));
			self.obj().append(&but);
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
	
	pub fn setMax(&self, max: u32)
	{
		self.max.set(max);
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
	}
	
	/**
	 Recurse through all child widgets and accumulate the StatefulButton:state()
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
