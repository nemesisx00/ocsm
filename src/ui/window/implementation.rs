use cofd::sheet::SheetCofdMortal;
use ctl2e::sheet::SheetCofdCtl2e;
use gtk4::gio::prelude::ListModelExt;
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
use widgets::traits::{CharacterSheet, Signal_SheetUpdated};
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
				move |_: HomeScreen, gameSystem: String| me.handleNewSheetPressed(gameSystem)
			)
		);
	}
	
	fn handleNewSheetPressed(&self, gameSystem: String)
	{
		let me = self;
		
		let pageName = format!("{}-{}", gameSystem, self.stack.pages().n_items());
		
		let page: Option<StackPage> = match gameSystem.as_str()
		{
			cofd::GameSystemId => {
				let sheet = SheetCofdMortal::new();
				sheet.setPageName(pageName.clone());
				
				sheet.connect_closure(
					Signal_SheetUpdated,
					false,
					closure_local!(
						#[weak] me,
						move |sheet: SheetCofdMortal| me.handleSheetUpdate(sheet)
					)
				);
				
				Some(self.stack.add_titled(&sheet, Some(pageName.as_str()), "New Mortal"))
			},
			
			ctl2e::GameSystemId => {
				let sheet = SheetCofdCtl2e::new();
				sheet.setPageName(pageName.clone());
				
				sheet.connect_closure(
					Signal_SheetUpdated,
					false,
					closure_local!(
						#[weak] me,
						move |sheet: SheetCofdCtl2e| me.handleSheetUpdate(sheet)
					)
				);
				
				Some(self.stack.add_titled(&sheet, Some(pageName.as_str()), "New Changeling"))
			},
			
			mta2e::GameSystemId => {
				let sheet = SheetCofdMta2e::new();
				sheet.setPageName(pageName.clone());
				
				sheet.connect_closure(
					Signal_SheetUpdated,
					false,
					closure_local!(
						#[weak] me,
						move |sheet: SheetCofdMta2e| me.handleSheetUpdate(sheet)
					)
				);
				
				Some(self.stack.add_titled(&sheet, Some(pageName.as_str()), "New Mage"))
			},
			
			vtr2e::GameSystemId => {
				let sheet = SheetCofdVtr2e::new();
				sheet.setPageName(pageName.clone());
				
				sheet.connect_closure(
					Signal_SheetUpdated,
					false,
					closure_local!(
						#[weak] me,
						move |sheet: SheetCofdVtr2e| me.handleSheetUpdate(sheet)
					)
				);
				
				Some(self.stack.add_titled(&sheet, Some(pageName.as_str()), "New Vampire"))
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
	
	fn handleSheetUpdate<T>(&self, sheet: T)
		where T: CharacterSheet
	{
		if let Some(p) = self.stack.child_by_name(sheet.pageName().as_str())
		{
			self.stack.page(&p).set_title(sheet.characterName().as_str());
		}
	}
}
