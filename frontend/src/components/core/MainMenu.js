import './MainMenu.css'
import React from 'react'
import { devSafeInvoke } from './DevSafeTauriInvoke'
const invoke = devSafeInvoke

const menuNameRegex = /.*?\s(.*?)\s.*/gi
const subMenuCloseTimeoutTime = 1500
let subMenuCloseTimeout = null

const MenuStates = Object.freeze({
	Hide: 'hide',
	Show: ''
})

export default class MainMenu extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			subMenus: {
				file: MenuStates.Hide
			}
		}
	}
	
	render()
	{
		return (
			<div className="mainMenu">
				<div className="menu file" onClick={(ev) => this.clickHandler_menu(ev)}>File</div>
				<div className={`subMenu file ${this.state.subMenus.file}`} onMouseEnter={(ev) => this.genericMouseEnterHandler(ev)} onMouseLeave={(ev) => this.genericMouseLeaveHandler(ev)}>
					<div className="menuItem" onClick={() => this.clickHandler_file_new()}>New</div>
					<div className="menuItem" onClick={() => this.clickHandler_file_open()}>Open</div>
					<div className="menuItem" onClick={() => this.clickHandler_file_exit()}>Exit</div>
				</div>
			</div>
		)
	}
	
	// Event Handlers ----------------------------------------
	
	clickHandler_menu(ev)
	{
		this.doClearSubmenuTimeout()
		
		let name = ev.target.classList ? ev.target.classList[1] : this.findMenuName(ev.target)
		if(name)
			this.switchSubmenuState(name.toLowerCase())
	}
	
	clickHandler_file_exit()
	{
		this.doClearSubmenuTimeout()
		this.hideSubmenu('file')
		
		invoke({ cmd: 'exitApp' })
	}
	
	clickHandler_file_new()
	{
		this.doClearSubmenuTimeout()
		this.hideSubmenu('file')
		
		invoke({ cmd: 'newSheet' })
	}
	
	clickHandler_file_open()
	{
		this.doClearSubmenuTimeout()
		this.hideSubmenu('file')
		
		invoke({ cmd: 'loadData', target: 'Documents' })
	}
	
	genericMouseEnterHandler()
	{
		this.doClearSubmenuTimeout()
	}
	
	genericMouseLeaveHandler(ev)
	{
		this.doClearSubmenuTimeout()
		
		subMenuCloseTimeout = setTimeout(() => {
			let name = ev.target.classList ? ev.target.classList[1] : this.findMenuName(ev.target)
			if(name)
				this.hideSubmenu(name.toLowerCase())
		}, subMenuCloseTimeoutTime)
	}
	
	// Helper Methods ----------------------------------------
	
	doClearSubmenuTimeout()
	{
		if(subMenuCloseTimeout)
			clearTimeout(subMenuCloseTimeout)
	}
	
	findMenuName(el)
	{
		let matches = menuNameRegex.exec(el)
		let name = null
		if(matches.length > 1)
			name = matches[1]
		return name
	}
	
	hideSubmenu(submenuName)
	{
		let newState = { subMenus: {...this.state.subMenus} }
		newState.subMenus[submenuName.toLowerCase()] = MenuStates.Hide
		
		this.setState(() => { return newState })
	}
	
	showSubmenu(submenuName)
	{
		let newState = { subMenus: {...this.state.subMenus} }
		newState.subMenus[submenuName.toLowerCase()] = MenuStates.Show
		
		this.setState(() => { return newState })
	}
	
	switchSubmenuState(menuName)
	{
		let newState = { subMenus: {...this.state.subMenus} }
		newState.subMenus[menuName.toLowerCase()] = this.state.subMenus[menuName.toLowerCase()] === MenuStates.Hide ? MenuStates.Show : MenuStates.Hide
		this.setState(() => { return newState })
	}
}
