use gtk4::{Box, CompositeTemplate, Label, SpinButton, TemplateChild};
use gtk4::glib::{self, closure_local};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::glib::types::StaticTypeExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;
use widgets::statefultrack::data::StateValue;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/experiences.ui")]
pub struct ExperiencesCofd
{
	#[template_child]
	beats: TemplateChild<StatefulTrack>,
	
	#[template_child]
	experiences: TemplateChild<SpinButton>,
	
	#[template_child]
	textLabel: TemplateChild<Label>,
}

impl BoxImpl for ExperiencesCofd {}

impl ObjectImpl for ExperiencesCofd
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		let me = self;
		self.beats.connect_closure(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.handleBeatsUpdate()
			)
		);
	}
}

#[glib::object_subclass]
impl ObjectSubclass for ExperiencesCofd
{
	const NAME: &'static str = "ExperiencesCofd";
	type ParentType = Box;
	type Type = super::ExperiencesCofd;
	
	fn class_init(klass: &mut Self::Class)
	{
		StatefulTrack::ensure_type();
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for ExperiencesCofd {}

impl ExperiencesCofd
{
	fn handleBeatsUpdate(&self)
	{
		if self.beats.value().one >= 5
		{
			self.experiences.set_value(self.experiences.value() + 1.0);
			self.beats.setValue(StateValue::default());
		}
	}
	
	pub fn setTextLabel(&self, label: String)
	{
		self.textLabel.set_label(label.as_str());
	}
}
