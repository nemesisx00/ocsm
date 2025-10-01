use std::cell::Cell;
use std::sync::OnceLock;
use gtk4::Grid;
use gtk4::prelude::{GridExt, WidgetExt};
use gtk4::glib::{self, closure_local, Properties};
use gtk4::glib::object::{CastNone, ObjectExt};
use gtk4::glib::subclass::Signal;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::prelude::DerivedObjectProperties;
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::prelude::StaticType;
use gtk4::subclass::grid::GridImpl;
use gtk4::subclass::widget::WidgetImpl;
use crate::button::stateful::{Signal_StateToggled, StatefulButton, StatefulMode};
use super::data::StateValue;

#[derive(Default, Properties)]
#[properties(wrapper_type = super::StatefulTrack)]
pub struct StatefulTrack
{
	#[property(construct, default = 5, get, set = Self::setMaximum)]
	maximum: Cell<u32>,
	
	#[property(construct, default = 0, get, set = Self::setMinimum)]
	minimum: Cell<u32>,
	
	#[property(construct, default = StatefulMode::CircleOne.into(), get, set = Self::setMode)]
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
	
	fn addNewButton(&self, index: u32) -> StatefulButton
	{
		let button = StatefulButton::withMode(self.mode.get());
		button.set_index(index);
		
		let rowLength = match self.rowLength.get() > 0
		{
			true => self.rowLength.get(),
			false => 10,
		};
		
		self.obj().attach(
			&button,
			(index % rowLength) as i32,
			(index / rowLength) as i32,
			1,
			1
		);
		
		button.connect_closure(
			Signal_StateToggled,
			false,
			closure_local!(
				#[weak(rename_to = this)] self,
				move |_: StatefulButton, index: u32| this.handleClick(index)
			)
		);
		
		button.refresh();
		
		return button;
	}
	
	#[allow(unused)]
	fn clear(&self)
	{
		self.value.set(StateValue::default());
		if let Some(child) = self.obj().first_child().and_downcast::<StatefulButton>()
		{
			self.clearChildState(child);
		}
	}
	
	fn clearChildState(&self, child: StatefulButton)
	{
		child.set_state(0);
		child.refresh();
		
		if let Some(next) = child.next_sibling().and_downcast::<StatefulButton>()
		{
			self.clearChildState(next);
		}
	}
	
	fn emitValue(&self)
	{
		let value = self.value.get();
		
		self.obj().emit_by_name::<()>(
			super::StatefulTrack::Signal_ValueUpdated,
			&[&value.one, &value.two, &value.three]
		);
	}
	
	pub fn getValue(&self) -> StateValue
	{
		return match self.obj().first_child().and_downcast::<StatefulButton>()
		{
			None => StateValue::default(),
			Some(first) => self.accumulateChildState(first, StateValue::default()),
		};
	}
	
	fn handleClick(&self, index: u32)
	{
		match self.mode.get().into()
		{
			StatefulMode::CircleOne | StatefulMode::BoxOne => {
				let indexValue = match self.value.get().one == index + 1
				{
					true => index,
					false => index + 1,
				};
				
				self.setValue(StateValue {
					one: indexValue,
					..Default::default()
				});
			},
			
			/*
			If it's not one of the single-state modes, then we
			need to update the value by reading from the current
			state of each button before refreshing.
			*/
			_ => self.updateValue()
		}
		
		self.emitValue();
		self.refresh();
	}
	
	pub fn refresh(&self)
	{
		let child = self.obj().first_child().and_downcast::<StatefulButton>();
		self.regenerateChildStates(child, 0, self.value.get());
	}
	
	fn regenerateChildStates(&self, child: Option<StatefulButton>, index: u32, prevValue: StateValue)
	{
		let nextIndex = index + 1;
		match child
		{
			None => {
				if index < self.maximum.get()
				{
					let button = self.addNewButton(index);
					
					let mut state = 0;
					let mut value = prevValue;
					
					if value.three > 0
					{
						state = 3;
						value.three = value.three - 1;
					}
					else if value.two > 0
					{
						state = 2;
						value.two = value.two - 1;
					}
					else if value.one > 0
					{
						state = 1;
						value.one = value.one - 1;
					}
					
					button.set_state(state);
					button.refresh();
					
					self.regenerateChildStates(None, nextIndex, prevValue);
				}
			},
			
			Some(button) => {
				if index >= self.maximum.get()
				{
					let next = button.next_sibling().and_downcast::<StatefulButton>();
					button.unparent();
					self.regenerateChildStates(next, nextIndex, prevValue);
				}
				else
				{
					let mut value = prevValue;
					
					let mut state = 0;
					if value.three > 0
					{
						state = 3;
						value.three = value.three - 1;
					}
					else if value.two > 0
					{
						state = 2;
						value.two = value.two - 1;
					}
					else if value.one > 0
					{
						state = 1;
						value.one = value.one - 1;
					}
					
					button.set_mode(self.mode.get());
					button.set_state(state);
					button.refresh();
					
					if index < self.maximum.get()
					{
						self.regenerateChildStates(button.next_sibling().and_downcast::<StatefulButton>(), nextIndex, value);
					}
				}
			}
		}
	}
	
	fn removeChildPosition(&self, child: StatefulButton, children: &mut Vec<StatefulButton>) -> Vec<StatefulButton>
	{
		let next = child.next_sibling().and_downcast::<StatefulButton>();
		
		self.obj().remove(&child);
		children.push(child);
		
		return match next
		{
			None => children.clone(),
			Some(c) => self.removeChildPosition(c, children),
		};
	}
	
	pub fn setMaximum(&self, max: u32)
	{
		self.maximum.set(max);
		
		let mut value = self.value.get();
		value.truncateMin(max, self.minimum.get());
		
		self.setValue(value);
	}
	
	pub fn setMinimum(&self, min: u32)
	{
		self.minimum.set(min);
		
		let mut value = self.value.get();
		value.truncateMin(self.maximum.get(), min);
		
		self.setValue(value);
	}
	
	pub fn setMode(&self, mode: u32)
	{
		self.mode.set(mode);
		self.refresh();
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.rowLength.set(length);
		
		let children = match self.obj().first_child().and_downcast::<StatefulButton>()
		{
			Some(child) => self.removeChildPosition(child, &mut vec![]),
			None => vec![],
		};
		
		let rowLength = match self.rowLength.get() > 0
		{
			true => self.rowLength.get(),
			false => 10,
		};
		
		for child in children
		{
			let index = child.index();
			
			self.obj().attach(
				&child,
				(index % rowLength) as i32,
				(index / rowLength) as i32,
				1,
				1
			);
		}
	}
	
	pub fn setValue(&self, value: StateValue)
	{
		let mut val = value;
		val.truncateMin(self.maximum.get(), self.minimum.get());
		self.value.set(val);
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
}
