import './MainMenu.css'
import React from 'react'
import { invoke } from 'tauri/api/tauri'

const MenuStates = Object.freeze({
	Hide: '',
	Show: 'show'
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
				<div className={`subMenu file ${this.state.subMenus.file}`}>
					<div className="menuItem" onClick={(ev) => this.clickHandler_file_exit(ev)}>Exit</div>
				</div>
			</div>
		)
	}
	
	clickHandler_menu(ev)
	{
		let menuName = ev.target.innerText.toLowerCase()
		let newState = { subMenus: {} }
		
		newState.subMenus[menuName] = this.state.subMenus[menuName] === MenuStates.Hide ? MenuStates.Show : MenuStates.Hide
		
		this.setState(() => {
			return newState
		})
	}
	
	clickHandler_file_exit(ev)
	{
		console.log('Exit clicked!')
		this.setState(() => {
			return { subMenus: { file: MenuStates.Hide } }
		})
		
		invoke({
			cmd: 'exitApp'
		})
	}
}
