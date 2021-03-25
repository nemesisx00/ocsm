import './Tabs.css'
import React from 'react'
import {normalizeClassNames} from './Utilities'

const TabClassStub = 'tab-'
const TabStates = Object.freeze({
	Show: 'show',
	Hide: 'hidden',
	Selected: 'selected'
})

export default class Tabs extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.state = {
			index: 0
		}
	}
	
	render()
	{
		//TODO: Come up with a way to label the tabs
		let tabs = []
		let tabBodies = []
		
		React.Children.forEach(this.props.children, (child, index) => {
			let tabClass = `${TabClassStub}${index}`
			tabs.push((
				<div
					key={tabClass}
					className={normalizeClassNames(tabClass, (index === this.state.index ? TabStates.Selected : ''))}
					onClick={() => this.tabClicked(index)}
				>{index}</div>
			))
			tabBodies.push((
				<div key={tabClass} className={normalizeClassNames(tabClass, (index === this.state.index ? TabStates.Show : TabStates.Hide))}>{child}</div>
			))
		})
		
		return (
			<div className={normalizeClassNames('tabs', (!!this.props.className ? this.props.className : ''))}>
				<div className="tabControls">
					{tabs}
				</div>
				<div className="tabContainer">
					{tabBodies}
				</div>
			</div>
		)
	}
	
	tabClicked(index)
	{
		this.setState({ index })
	}
}
