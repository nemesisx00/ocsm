use cofd::sheet::SheetCofdMortal;
use ctl2e::sheet::SheetCofdCtl2e;
use gtk4::glib::object::ObjectExt;
use gtk4::{CompositeTemplate, Stack, StackPage, TemplateChild};
use glib::subclass::InitializingObject;
use gtk4::glib::{self, closure_local};
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::glib::types::StaticTypeExt;
use gtk4::prelude::GtkWindowExt;
use gtk4::subclass::prelude::ApplicationWindowImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use gtk4::subclass::window::WindowImpl;
use mta2e::sheet::SheetCofdMta2e;
use vtr2e::sheet::SheetCofdVtr2e;
use crate::ui::home::{HomeScreen, Signal_NewSheet};

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/window.ui")]
pub struct OcsmWindow
{
	#[template_child]
	homeScreen: TemplateChild<HomeScreen>,
	
	#[template_child]
	stack: TemplateChild<Stack>,
}

impl ApplicationWindowImpl for OcsmWindow {}

impl ObjectImpl for OcsmWindow
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.obj().set_default_size(640, 480);
		
		self.stack.set_transition_duration(300);
		self.stack.set_transition_type(gtk4::StackTransitionType::UnderDown);
		
		self.connectHandlers();
	}
}

#[glib::object_subclass]
impl ObjectSubclass for OcsmWindow
{
	// `NAME` needs to match `class` attribute of template
	const NAME: &'static str = "OcsmWindow";
	type Type = super::OcsmWindow;
	type ParentType = gtk4::ApplicationWindow;
	
	fn class_init(klass: &mut Self::Class)
	{
		HomeScreen::ensure_type();
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for OcsmWindow {}
impl WindowImpl for OcsmWindow {}

impl OcsmWindow
{
	fn connectHandlers(&self)
	{
		let me = self;
		
		self.homeScreen.connect_closure(
			Signal_NewSheet,
			false,
			closure_local!(
				#[weak] me,
				move |_: HomeScreen, gameSystem: u32| me.handleNewSheetPressed(gameSystem)
			)
		);
	}
	
	fn handleNewSheetPressed(&self, gameSystem: u32)
	{
		let page: Option<StackPage> = match gameSystem
		{
			0 => {
				let sheet = SheetCofdMortal::new();
				Some(self.stack.add_titled(&sheet, Some("cofdMortal"), "Mortal"))
			},
			
			1 => {
				let sheet = SheetCofdCtl2e::new();
				Some(self.stack.add_titled(&sheet, Some("cofdCtl2e"), "Changeling"))
			},
			
			2 => {
				let sheet = SheetCofdMta2e::new();
				Some(self.stack.add_titled(&sheet, Some("cofdMta2e"), "Mage"))
			},
			
			3 => {
				let sheet = SheetCofdVtr2e::new();
				Some(self.stack.add_titled(&sheet, Some("cofdVtr2e"), "Vampire"))
			},
			
			_ => None,
		};
		
		if let Some(p) = page
		{
			if let Some(name) = p.name()
			{
				self.stack.set_visible_child_full(
					&name,
					gtk4::StackTransitionType::OverUp
				);
			}
		}
	}
}
