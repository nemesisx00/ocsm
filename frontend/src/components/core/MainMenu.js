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
					<div className="menuItem" onClick={(ev) => this.clickHandler_file_exit(ev)}>Exit</div>
				</div>
			</div>
		)
	}
	
	clickHandler_menu(ev)
	{
		if(subMenuCloseTimeout)
			clearTimeout(subMenuCloseTimeout)
		
		let name = ev.target.classList ? ev.target.classList[1] : this.findMenuName(ev.target)
		if(name)
			this.switchSubmenuState(name.toLowerCase())
	}
	
	clickHandler_file_exit()
	{
		if(subMenuCloseTimeout)
			clearTimeout(subMenuCloseTimeout)
		
		this.setState(() => {
			return { subMenus: { file: MenuStates.Hide } }
		})
		
		invoke({ cmd: 'exitApp' })
	}
	
	genericMouseEnterHandler()
	{
		if(subMenuCloseTimeout)
			clearTimeout(subMenuCloseTimeout)
	}
	
	genericMouseLeaveHandler(ev)
	{
		if(subMenuCloseTimeout)
			clearTimeout(subMenuCloseTimeout)
		
		subMenuCloseTimeout = setTimeout(() => {
			let name = ev.target.classList ? ev.target.classList[1] : this.findMenuName(ev.target)
			if(name)
				this.switchSubmenuState(name.toLowerCase())
		}, subMenuCloseTimeoutTime)
	}
	
	findMenuName(el)
	{
		let matches = menuNameRegex.exec(el)
		let name = null
		if(matches.length > 1)
			name = matches[1]
		return name
	}
	
	switchSubmenuState(menuName)
	{
		let newState = { subMenus: {} }
		newState.subMenus[menuName] = this.state.subMenus[menuName] === MenuStates.Hide ? MenuStates.Show : MenuStates.Hide
		this.setState(() => { return newState })
	}
}
