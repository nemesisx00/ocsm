import './MainMenu.css'
import React from 'react'

export default class MainMenu extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
		}
	}
	
	render()
	{
		return (
			<div className="mainMenu">
				<div className="menu">Main</div>
			</div>
		)
	}
}
