import './App.css'
import React from 'react'
import ChangelingTheLost from './components/wod/changeling/ChangelingTheLost'
import MainMenu from './components/core/MainMenu'

export default class App extends React.Component
{
	render()
	{
		return (
			<div className="App">
				<MainMenu />
				<ChangelingTheLost />
			</div>
		)
	}
}
