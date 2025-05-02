use std::cell::Cell;
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
	#[property(construct, default = 5, get, set = StatefulTrack::setMax)]
	max: Cell<u32>,
	
	#[property(construct, default = StatefulMode::CircleOne.into(), get, set)]
	mode: Cell<u32>,
	
	#[property(construct, default = 10, get, set)]
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
						},
						
						_ => {},
					}
					
					me.refresh();
					me.updateValue();
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
