import './App.css'
import { listen } from '@tauri-apps/api/event'
import React from 'react'
import ChangelingTheLost from './components/wod/changeling/ChangelingTheLost'
import MageTheAwakening from './components/wod/mage/MageTheAwakening'
import VampireTheMasquerade from './components/wod/vampire/VampireTheMasquerade'

const Contexts = Object.freeze({
	WoD: {
		CtL: 'ChangelingTheLost',
		MtA: 'MageTheAwakening',
		VtM: 'VampireTheMasquerade'
	}
})

class App extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.state = {
			context: null
		}
		
		this.listenerHandles = {
			newSheet: null
		}
	}
	
	componentDidMount()
	{
		this.listenerHandles.newSheet = listen('newSheet', (event) => this.newSheetHandler(event))
	}
	
	render()
	{
		let sheet = null
		switch(this.state.context)
		{
			case Contexts.WoD.CtL:
				sheet = (<ChangelingTheLost />)
				break
			case Contexts.WoD.MtA:
				sheet = (<MageTheAwakening />)
				break
			case Contexts.WoD.VtM:
				sheet = (<VampireTheMasquerade />)
				break
			default:
				sheet = (<h1>At some point there will probably be a default UI element here but for now just use the main menu to select a new sheet.</h1>)
				sheet = (<MageTheAwakening />)
		}
		
		return (
			<div className="App">
				{sheet}
			</div>
		)
	}
	
	newSheetHandler(event)
	{
		if(event.payload.context)
		{
			this.setState(() => {
				return { context: event.payload.context }
			})
		}
	}
}

App.Contexts = Contexts

export default App
