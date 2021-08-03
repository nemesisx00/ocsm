import './App.css'
import React from 'react'
//import ChangelingTheLost from './components/wod/changeling/ChangelingTheLost'
import VampireTheMasquerade from './components/wod/vampire/VampireTheMasquerade'

export default class App extends React.Component
{
	render()
	{
		return (
			<div className="App">
				<VampireTheMasquerade />
			</div>
		)
		//<ChangelingTheLost />
	}
}
