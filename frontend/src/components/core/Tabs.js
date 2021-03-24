import './Tabs.css'
import React from 'react'

const TabClassStub = 'tab-'
const TabStates = Object.freeze({
	Show: 'show',
	Hide: 'hidden',
	Selected: 'selected'
})

function normalizeClassNames(className, ...additions)
{
	let out = []
	if(className && typeof className === 'string')
		className.split(' ').forEach(word => out.push(word.trim()))
	if(additions)
		additions.forEach(word => { word.split(' ').forEach(w => out.push(w.trim())) })
	return out.filter(val => val.length > 0).join(' ').trim()
}

export default class Tabs extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.state = {
			index: 0,
			parentClassName: props.className
		}
	}
	
	render()
	{
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
			<div className={`tabs ${this.state.parentClassName}`}>
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
