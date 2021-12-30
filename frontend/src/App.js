import './App.css'
import { listen } from '@tauri-apps/api/event'
import { invoke } from '@tauri-apps/api/tauri'
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
const DefaultContext = Contexts.WoD.MtA
const DefaultSheetState = MageTheAwakening.EmptySheet

class App extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.state = {
			context: DefaultContext,
			sheetState: DefaultSheetState
		}
		
		this.listenerHandles = {
			newSheet: null,
			loadSheet: null,
			saveSheet: null
		}
	}
	
	componentDidMount()
	{
		this.listenerHandles.newSheet = listen('newSheet', (event) => this.newSheetHandler(event))
		this.listenerHandles.loadSheet = listen('loadSheet', (obj) => this.loadSheetHandler(obj))
		this.listenerHandles.saveSheet = listen('saveSheet', () => this.saveSheetHandler())
	}
	
	render()
	{
		//TODO: Remove this console.log call when it is no longer necessary
		console.log(this.state.sheetState)
		let sheet = null
		switch(this.state.context)
		{
			case Contexts.WoD.CtL:
				sheet = (<ChangelingTheLost sheetState={this.state.sheetState} updateSheetState={(sheetState) => this.updateSheetStateHandler(sheetState)} />)
				break
			case Contexts.WoD.MtA:
				sheet = (<MageTheAwakening sheetState={this.state.sheetState} updateSheetState={(sheetState) => this.updateSheetStateHandler(sheetState)} />)
				break
			case Contexts.WoD.VtM:
				sheet = (<VampireTheMasquerade sheetState={this.state.sheetState} updateSheetState={(sheetState) => this.updateSheetStateHandler(sheetState)} />)
				break
			default:
				
		}
		
		return (
			<div className="App">
				{sheet}
			</div>
		)
	}
	
	getEmptySheet(context)
	{
		switch(context)
		{
			case Contexts.WoD.CtL:
				return ChangelingTheLost.EmptySheet
			case Contexts.WoD.MtA:
				return MageTheAwakening.EmptySheet
			case Contexts.WoD.VtM:
				return VampireTheMasquerade.EmptySheet
			default:
				return {}
		}
	}
	
	getSheetSortMethod(context)
	{
		switch(context)
		{
			case Contexts.WoD.MtA:
				return MageTheAwakening.SortState
			default:
				return state => state
		}
	}
	
	loadSheetHandler(obj)
	{
		if(obj && obj.payload)
		{
			let payload = null
			try { payload = JSON.parse(obj.payload) }
			catch(err) { console.log(`Failed to parse the loaded sheet: ${err}`) }
			
			if(payload !== null)
			{
				let sheetState = JSON.parse(payload.sheetState)
				let sorter = this.getSheetSortMethod(payload.context)
				
				this.setState(() => { return {
						context: payload.context,
						sheetState: sorter(sheetState)
					}
				})
			}
		}
	}
	
	newSheetHandler(event)
	{
		if(event.payload.context)
		{
			let newState = {
				context: event.payload.context,
				sheetState: this.getEmptySheet(event.payload.context)
			}
			
			this.setState(() => { return newState })
		}
	}
	
	saveSheetHandler()
	{
		console.log('Invoking backend SaveSheet event!')
		invoke('SaveSheet', { context: this.state.context, state: JSON.stringify(this.state.sheetState) })
	}
	
	updateSheetStateHandler(sheetState)
	{
		let newState = {...this.state}
		newState.sheetState = sheetState
		this.setState(() => { return newState })
	}
}

App.Contexts = Contexts

export default App
