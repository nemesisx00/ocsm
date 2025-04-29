use glib::subclass::InitializingObject;
use gtk4::prelude::*;
use gtk4::subclass::prelude::*;
use gtk4::{glib, Button, CompositeTemplate};
use widgets::statefultrack::data::StateValue;
use widgets::statefultrack::StatefulTrack;
use crate::ui::nav::LeftNav;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/window.ui")]
pub struct OcsmWindow
{
	#[template_child]
	pub button: TemplateChild<Button>,
	
	#[template_child]
	pub boxes3: TemplateChild<StatefulTrack>,
}

impl ApplicationWindowImpl for OcsmWindow {}

#[glib::object_subclass]
impl ObjectSubclass for OcsmWindow
{
	// `NAME` needs to match `class` attribute of template
	const NAME: &'static str = "OcsmWindow";
	type Type = super::OcsmWindow;
	type ParentType = gtk4::ApplicationWindow;
	
	fn class_init(klass: &mut Self::Class)
	{
		StatefulTrack::ensure_type();
		LeftNav::ensure_type();
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl ObjectImpl for OcsmWindow
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.boxes3.setValue(StateValue {
			one: 3,
			two: 1,
			three: 2,
		});
		
		let boxes = self.boxes3.clone();
		self.button.connect_clicked(move |button|
		{
			button.set_label("Hello World!");
			println!("Dots value: {:?}", boxes.value());
		});
	}
}

impl WidgetImpl for OcsmWindow {}
impl WindowImpl for OcsmWindow {}
