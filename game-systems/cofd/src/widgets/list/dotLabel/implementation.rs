use std::sync::OnceLock;

use gtk4::glib::object::ObjectExt;
use gtk4::prelude::{EditableExt, EntryExt, WidgetExt};
use gtk4::{Box, CompositeTemplate, Entry, TemplateChild};
use gtk4::glib::{self, clone, closure_local, Properties};
use gtk4::glib::subclass::{InitializingObject, Signal};
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::prelude::DerivedObjectProperties;
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::data::StateValue;
use widgets::statefultrack::StatefulTrack;

#[derive(CompositeTemplate, Default, Properties)]
#[properties(wrapper_type = super::DotLabel)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/dotLabel.ui")]
pub struct DotLabel
{
	#[property(
		get = Self::labelText,
		set = Self::setLabelText,
		type = String,
	)]
	#[template_child]
	pub label: TemplateChild<Entry>,
	
	#[property(
		get = Self::value,
		set = Self::setValue,
		type = u32
	)]
	#[template_child]
	pub value: TemplateChild<StatefulTrack>,
}

impl BoxImpl for DotLabel {}

#[glib::derived_properties]
impl ObjectImpl for DotLabel
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		let me = self;
		
		self.label.connect_activate(clone!(
			#[weak] me,
			move |_| me.obj().emit_by_name::<()>(super::DotLabel::Signal_DotLabelChanged, &[])
		));
		
		//TODO: Figure out why this doesn't seem to be triggering
		self.label.connect_move_focus(clone!(
			#[weak] me,
			move |_, _| me.obj().emit_by_name::<()>(super::DotLabel::Signal_DotLabelChanged, &[])
		));
		
		self.value.connect_closure(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.obj().emit_by_name::<()>(super::DotLabel::Signal_DotLabelChanged, &[])
			)
		);
	}
	
	fn signals() -> &'static [Signal]
	{
		static SIGNALS: OnceLock<Vec<Signal>> = OnceLock::new();
		return SIGNALS.get_or_init(|| {
			vec![
				Signal::builder(super::DotLabel::Signal_DotLabelChanged)
					.build()
			]
		});
	}
}

#[glib::object_subclass]
impl ObjectSubclass for DotLabel
{
	const NAME: &'static str = "DotLabel";
	type ParentType = Box;
	type Type = super::DotLabel;
	
	fn class_init(klass: &mut Self::Class)
	{
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for DotLabel {}

impl DotLabel
{
	fn labelText(&self) -> String
	{
		return self.label.text().into();
	}
	
	fn setLabelText(&self, text: String)
	{
		self.label.set_text(text.as_str());
	}
	
	fn setValue(&self, value: u32)
	{
		self.value.setValue(StateValue { one: value, ..Default::default() });
	}
	
	fn value(&self) -> u32
	{
		return self.value.value().one;
	}
}
